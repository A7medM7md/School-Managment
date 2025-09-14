using Microsoft.AspNetCore.Identity;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AuthorizationService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            var role = new IdentityRole<int> { Name = roleName };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
