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


            var (accessToken, jti) = await GenerateAccessToken(user);

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
                UserName = user.UserName ?? string.Empty
            };
        }


        private async Task<(string, string)> GenerateAccessToken(AppUser user)
        {
            // Generate Signing Credentials
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

            var jti = Guid.NewGuid().ToString();

            // Build Claims
            var claims = await BuildClaims(user, jti);

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


        private async Task<IEnumerable<Claim>> BuildClaims(AppUser user, string jti)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty), // for [Authorize]
                new Claim(JwtRegisteredClaimNames.Jti, jti),
            };

            if (!string.IsNullOrWhiteSpace(user.Email)) // Check Due To Cannot Be Added Empty
            {
                authClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                authClaims.Add(new Claim(ClaimTypes.Email, user.Email)); // for [Authorize]
            }

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber)) // Check Due To Cannot Be Added Empty
            {
                authClaims.Add(new Claim("phone_number", user.PhoneNumber));
            }

            // User Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Custome User Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Here I Use Loop and Add() Instead Of AddRange() To Make It Readable With Condition.., But You Can Use AddRange() Like Roles..
            foreach (var userClaim in userClaims)
            {
                // Check To Avoid If There Is Duplicate Claims In Custom and Identity Registered Claims
                if (!authClaims.Any(c => c.Type == userClaim.Type && c.Value == userClaim.Value))
                    authClaims.Add(new Claim(userClaim.Type, userClaim.Value));
            }

            return authClaims;
        }


        public async Task<TokenServiceResult<SignInResponse>> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                // 1. Decode Payload Of Expired Access Token
                var jwtToken = ReadAccessToken(accessToken);

                if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                    return TokenServiceResult<SignInResponse>.Fail("InvalidAccessToken", 401);

                // Extract UserId from claims
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return TokenServiceResult<SignInResponse>.Fail("UserIdInAccessTokenNotFound", 404);

                // 2. Check Refresh Token in DB
                var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

                if (storedRefreshToken == null ||
                    storedRefreshToken.UserId.ToString() != userId ||
                    storedRefreshToken.ExpireAt <= DateTime.UtcNow ||
                    storedRefreshToken.IsRevoked)
                {
                    return TokenServiceResult<SignInResponse>.Fail("InvalidRefreshToken", 401);
                }

                // 3. Get User from DB
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return TokenServiceResult<SignInResponse>.Fail("UserIsNotFound", 404);

                // 4. Generate New Access Token
                var (newAccessToken, jti) = await GenerateAccessToken(user);

                /// 5. (Optional) Renew Refresh Token
                ///var newRefreshToken = GenerateRefreshToken();
                ///storedRefreshToken.Token = newRefreshToken;
                ///storedRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
                ///await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

                storedRefreshToken.JwtId = jti;
                storedRefreshToken.IsUsed = true;
                await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

                // 6. Return Response
                var response = new SignInResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = new RefreshTokenResponse
                    {
                        Token = refreshToken,
                        ExpireAt = storedRefreshToken.ExpireAt

                    },
                    UserName = user.UserName ?? string.Empty
                };

                return TokenServiceResult<SignInResponse>.Ok(response);
            }
            catch (Exception ex)
            {
                return TokenServiceResult<SignInResponse>.Fail("Unexpected error: " + ex.Message, 400);
            }
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


        public async Task<TokenServiceResult<TokenValidationResponse>> ValidateAccessToken(string accessToken, bool ignoreExpiry = false)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                return TokenServiceResult<TokenValidationResponse>.Fail("InvalidAccessToken", 400);

            var jwtHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
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

                var claims = principal.Claims.Select(c => new ClaimResponse
                {
                    Type = c.Type,
                    Value = c.Value
                }).ToList();

                var response = new TokenValidationResponse
                {
                    IsValid = true,
                    UserId = userId,
                    UserName = userName,
                    Claims = claims,
                    SecurityTokenType = validatedToken.GetType().Name,
                    ValidFrom = validatedToken.ValidFrom,
                    ValidTo = validatedToken.ValidTo
                };

                return TokenServiceResult<TokenValidationResponse>.Ok(response);
            }
            catch (Exception ex)
            {
                var response = new TokenValidationResponse
                {
                    IsValid = false,
                    ErrorMessage = ex.Message
                };

                return TokenServiceResult<TokenValidationResponse>.Fail(
                    "InvalidAccessToken",
                    401,
                    response
                );
            }
        }

    }
}
