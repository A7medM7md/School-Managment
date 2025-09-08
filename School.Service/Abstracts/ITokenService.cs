using School.Data.Entities.Identity;

namespace School.Service.Abstracts
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtTokenAsync(AppUser user);
    }
}
