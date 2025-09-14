using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Authorization.Commands.Models;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin")] // If User Is Not Admin Return => Forbidden
    public class AuthorizationController : AppBaseController
    {
        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<ActionResult<Response<string>>> AddRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }


}
