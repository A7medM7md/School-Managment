using School.Data.Entities.Identity;

namespace School.Service.Abstracts
{
    public interface ICurrentUserService
    {
        public Task<AppUser> GetCurrentUserAsync();
        public int GetCurrentUserId();
        public Task<IList<string>> GetCurrentUserRolesAsync();
    }
}
