using School.Data.Entities.Identity;
using School.Data.Helpers.JWT;

namespace School.Service.Abstracts
{
    public interface ITokenService
    {
        public Task<SignInResponse> GenerateJwtTokenAsync(AppUser user);
    }
}
