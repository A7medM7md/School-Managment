using Microsoft.AspNetCore.Identity;
using School.Data.Dtos;
using School.Service.Responses;

namespace School.Service.Abstracts
{
    public interface IAuthorizationService
    {
        public Task<IdentityResult> AddRoleAsync(string roleName);
        public Task<IdentityResult> EditRoleAsync(int id, string roleName);
        public Task<RoleResult> DeleteRoleAsync(int id);
        public Task<IdentityResult> AssignRoleAsync(int userId, string roleName);
        public Task<bool> IsRoleExist(string roleName, int? excludeId = null);

        public Task<IReadOnlyList<IdentityRole<int>>> GetRolesAsync(CancellationToken cancellationToken);
        public Task<IdentityRole<int>?> GetRoleByIdAsync(int id);
        public Task<IReadOnlyList<RoleDto>> GetRolesForUserAsync(int userId);

        public Task<IdentityResult> UpdateUserRolesAsync(int userId, IReadOnlyList<RoleDto> roles);

    }
}
