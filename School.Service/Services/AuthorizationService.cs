using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Data.Dtos;
using School.Data.Entities.Identity;
using School.Service.Abstracts;
using School.Service.Responses;

namespace School.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthorizationService(RoleManager<IdentityRole<int>> roleManager,
                                        UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            var role = new IdentityRole<int>(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<bool> IsRoleExist(string roleName, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _roleManager.Roles
                    .AsNoTracking()
                    .AnyAsync(r => r.Name == roleName && r.Id != excludeId.Value);
            }
            else
            {
                return await _roleManager.RoleExistsAsync(roleName);
            }
        }

        public async Task<IdentityResult> AssignRoleAsync(int userId, string roleName)
        {
            // Get user
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            // Assign role
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> EditRoleAsync(int id, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);

            return result;
        }


        public async Task<RoleResult> DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
                return new RoleResult { Status = RoleStatus.NotFound };

            var roleUsers = await _userManager.GetUsersInRoleAsync(role.Name!);

            if (roleUsers.Any())
                return new RoleResult { Status = RoleStatus.HasUsers, Role = role };

            var result = await _roleManager.DeleteAsync(role);

            return new RoleResult
            {
                Status = result.Succeeded ? RoleStatus.Success : RoleStatus.Failed,
                IdentityResult = result
            };
        }


        public async Task<IReadOnlyList<IdentityRole<int>>> GetRolesAsync(CancellationToken cancellationToken)
        {
            return await _roleManager.Roles
                            .AsNoTracking()
                            .ToListAsync(cancellationToken);
        }

        public async Task<IdentityRole<int>?> GetRoleByIdAsync(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IReadOnlyList<RoleDto>> GetRolesForUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Array.Empty<RoleDto>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var allRoles = await _roleManager.Roles.AsNoTracking().ToListAsync();

            var result = allRoles.Select(role => new RoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name!,
                HasRole = userRoles.Contains(role.Name!)
            })
            .ToList();

            return result;
        }



    }
}
