using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Authorization.Commands.Models;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin,User")] // If User Is Not Admin Return => 403 Forbidden
    public class AuthorizationController : AppBaseController
    {
        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<ActionResult<Response<string>>> AddRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthorizationRouting.AssignRole)]
        public async Task<ActionResult<Response<string>>> AddRole([FromForm] AssignRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }


}
