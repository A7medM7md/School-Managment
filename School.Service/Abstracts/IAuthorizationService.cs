using Microsoft.AspNetCore.Identity;

namespace School.Service.Abstracts
{
    public interface IAuthorizationService
    {
        public Task<IdentityResult> AddRoleAsync(string roleName);
        public Task<IdentityResult> EditRoleAsync(int id, string roleName);
        public Task<bool> IsRoleExist(string roleName, int? excludeId = null);
        public Task<IdentityResult> AssignRoleAsync(int userId, string roleName);

    }
}
