using Microsoft.IdentityModel.Tokens;
using School.Data.Entities.Identity;
using School.Data.Helpers.JWT;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;
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

        public TokenService(JWTSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<SignInResponse> GenerateJwtTokenAsync(AppUser user)
        {
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

        private (string Token, string Jti) GenerateAccessToken(AppUser user)
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


    }
}
