using Microsoft.AspNetCore.Identity;
using School.Data.Entities.Identity;
using School.Service.Abstracts;

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

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> AssignRoleAsync(int userId, string roleName)
        {
            // Get user
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found");

            // Assign role
            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
