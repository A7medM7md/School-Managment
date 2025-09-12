using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using School.Data.Entities.Identity;
using School.Data.Helpers.JWT;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;
using School.Service.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(JWTSettings jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            UserManager<AppUser> userManager)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }

        public async Task<SignInResponse> GenerateJwtTokenAsync(AppUser user)
        {
            // Revoke old refresh tokens
            var oldTokens = _refreshTokenRepository.GetTableAsTracking()
                .Where(rt => rt.UserId == user.Id && !rt.IsRevoked && rt.ExpireAt > DateTime.UtcNow)
                .ToList(); // materialize the query

            foreach (var token in oldTokens)
            {
                token.IsRevoked = true;
                token.IsUsed = false;
            }

            await _refreshTokenRepository.SaveChangesAsync();


            var (accessToken, jti) = GenerateAccessToken(user);

            var refreshToken = GenerateRefreshToken(jti, user.Id);

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new SignInResponse
            {
                AccessToken = accessToken,
                RefreshToken = new RefreshTokenResponse
                {
                    Token = refreshToken.Token,
                    ExpireAt = refreshToken.ExpireAt
                },
                UserName = user.UserName
            };
        }


        private (string, string) GenerateAccessToken(AppUser user)
        {
            // Generate Signing Credentials
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var jti = Guid.NewGuid().ToString();

            // Build Claims
            var claims = BuildClaims(user, jti);

            // Create JWT Token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryAccessTokenMinutes),
                signingCredentials: signingCredentials
            );

            // Return Encoded Access Token
            return (new JwtSecurityTokenHandler().WriteToken(token), jti);
        }


        private RefreshToken GenerateRefreshToken(string jti, int userId)
        {
            // Generate 512-bit secure random token
            // Array of bytes with random secure numbers
            var tokenBytes = RandomNumberGenerator.GetBytes(64); // cryptographically secure unlike random()
            var refreshToken = Convert.ToBase64String(tokenBytes); // convert bytes to string

            return new RefreshToken
            {
                Token = refreshToken,
                JwtId = jti,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryRefreshTokenDays),
            };
        }


        private static IEnumerable<Claim> BuildClaims(AppUser user, string jti)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim("phone_number", user.PhoneNumber ?? string.Empty)
            };
        }


        public async Task<SignInResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            // 1. Decode Payload Of Expired Access Token
            var jwtToken = ReadAccessToken(accessToken);

            if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            // Extract UserId from claims
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new SecurityTokenException("UserId not found in access token");

            // 2. Check Refresh Token in DB
            var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedRefreshToken == null ||
                storedRefreshToken.UserId.ToString() != userId ||
                storedRefreshToken.ExpireAt <= DateTime.UtcNow ||
                storedRefreshToken.IsRevoked)
            {
                throw new SecurityTokenException("Invalid or used refresh token");
            }

            // 3. Get User from DB
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new SecurityTokenException("User not found");

            // 4. Generate New Access Token
            var (newAccessToken, jti) = GenerateAccessToken(user);

            /// 5. (Optional) Renew Refresh Token
            ///var newRefreshToken = GenerateRefreshToken();
            ///storedRefreshToken.Token = newRefreshToken;
            ///storedRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            ///await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            storedRefreshToken.JwtId = jti;
            storedRefreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            // 6. Return Response
            return new SignInResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = new RefreshTokenResponse
                {
                    Token = refreshToken,
                    ExpireAt = storedRefreshToken.ExpireAt

                },
                UserName = user.UserName
            };
        }


        /// Decode expired access token to read claims (no validation on expiry)
        private JwtSecurityToken ReadAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            var jwtHandler = new JwtSecurityTokenHandler();

            // Read token without validating expiry
            var token = jwtHandler.ReadJwtToken(accessToken);

            return token;
        }


        public async Task<TokenValidationResponse> ValidateAccessToken(string accessToken, bool ignoreExpiry = false)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return new TokenValidationResponse
                {
                    IsValid = false,
                    ErrorMessage = "Access token is required."
                };
            }

            var jwtHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = !_jwtSettings.ValidateIssuer,
                ValidIssuer = _jwtSettings.Issuer,

                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),

                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidAudience = _jwtSettings.Audience,

                // We can choose to ignore the token's expiration when validating, 
                // which is useful in the refresh token flow. 
                // This allows reading claims from an expired access token without throwing an exception.
                ValidateLifetime = !ignoreExpiry,
                ClockSkew = TimeSpan.Zero
            };


            try
            {
                // ValidateToken => Throw Exception Automatically If Validation Failed
                var principal = jwtHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

                var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                             ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var userName = principal.Identity?.Name
                               ?? principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;

                var claims = principal.Claims.Select(c => new ClaimDto
                {
                    Type = c.Type,
                    Value = c.Value
                }).ToList();

                return new TokenValidationResponse
                {
                    IsValid = true,
                    UserId = userId,
                    UserName = userName,
                    Claims = claims,
                    SecurityTokenType = validatedToken.GetType().Name,
                    ValidFrom = validatedToken.ValidFrom,
                    ValidTo = validatedToken.ValidTo
                };
            }
            catch (Exception ex)
            {
                return new TokenValidationResponse
                {
                    IsValid = false,
                    ErrorMessage = ex.Message
                };
            }
        }


    }
}
