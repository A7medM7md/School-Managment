using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using School.Service.Abstracts;

namespace School.Core.Filters
{
    public class ValidateUserPermissionsFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;

        public ValidateUserPermissionsFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Before Action Executes

            if (context?.HttpContext?.User?.Identity?.IsAuthenticated is true)
            {
                var userRoles = await _currentUserService.GetCurrentUserRolesAsync();

                if (userRoles == null || !userRoles.Contains("User"))
                {
                    context.Result = new ObjectResult("")
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };

                    return;
                }
            }

            await next();

            // After Action Executes [Nothing To Do Here]
        }

    }
}
