using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using School.Data.Entities.Identity;
using School.Service.Abstracts;
using System.Security.Claims;

namespace School.Service.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
                                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID claim not found.");

            if (!int.TryParse(userIdClaim, out int userId))
                throw new InvalidOperationException("User ID claim is not a valid integer.");

            return userId;
        }

        public async Task<AppUser> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            return user;
        }

        public async Task<IList<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetCurrentUserAsync();

            var userRoles = await _userManager.GetRolesAsync(user);

            return userRoles;
        }
    }
}
