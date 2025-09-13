using School.Data.Entities.Identity;
using School.Data.Helpers.JWT;
using School.Service.Responses;

namespace School.Service.Abstracts
{
    public interface ITokenService
    {
        public Task<SignInResponse> GenerateJwtTokenAsync(AppUser user);
        public Task<TokenServiceResult<SignInResponse>> RefreshTokenAsync(string accessToken, string refreshToken);
        public Task<TokenServiceResult<TokenValidationResponse>> ValidateAccessToken(string accessToken, bool ignoreExpiry = false);

    }
}
