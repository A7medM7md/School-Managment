using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Features.Authorization.Queries.Models;
using School.Core.Features.Authorization.Queries.Responses;
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
        public async Task<ActionResult<Response<string>>> AssignRole([FromForm] AssignRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.AuthorizationRouting.EditRole)]
        public async Task<ActionResult<Response<string>>> EditRole([FromRoute] int id, [FromBody] EditRoleCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<ActionResult<Response<string>>> DeleteRole([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteRoleCommand(id));
            return NewResult(response);
        }


        [AllowAnonymous]
        [HttpGet(Router.AuthorizationRouting.GetRoles)]
        public async Task<ActionResult<Response<IReadOnlyList<GetRolesResponse>>>> GetRoles()
        {
            var response = await Mediator.Send(new GetRolesQuery());
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.AuthorizationRouting.GetRoleById)]
        public async Task<ActionResult<Response<GetRoleByIdResponse>>> GetRoleById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetRoleByIdQuery() { Id = id });
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.AuthorizationRouting.GetRolesForUser)] // Get All Roles But For This User Roles They Have a HasRole = True..
        public async Task<ActionResult<Response<GetRolesForUserResponse>>> GetRoleForUser([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetRolesForUserQuery() { UserId = id });
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPut(Router.AuthorizationRouting.UpdateUserRoles)]
        public async Task<ActionResult<Response<UpdateUserRolesCommand>>> UpdateUserRoles([FromRoute] int id, [FromBody] UpdateUserRolesCommand command)
        {
            command.UserId = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }


}
