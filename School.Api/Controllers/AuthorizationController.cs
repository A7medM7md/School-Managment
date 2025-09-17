using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Features.Authorization.Queries.Models;
using School.Core.Features.Authorization.Queries.Responses;
using School.Data.AppMetaData;
using School.Data.Commons;

namespace School.Api.Controllers
{
    //[Authorize(Roles = "Admin,User")] // If User Is Not Admin Return => 403 Forbidden
    public class AuthorizationController : AppBaseController
    {
        #region Role Endpoints

        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<IActionResult> AddRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.AuthorizationRouting.AssignRole)]
        public async Task<IActionResult> AssignRole([FromRoute] int id, [FromBody] AssignRoleCommand command)
        {
            command.UserId = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.AuthorizationRouting.EditRole)]
        public async Task<IActionResult> EditRole([FromRoute] int id, [FromBody] EditRoleCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteRoleCommand(id));
            return NewResult(response);
        }

        [HttpGet(Router.AuthorizationRouting.GetRoles)]
        public async Task<IActionResult> GetRoles()
        {
            var response = await Mediator.Send(new GetRolesQuery());
            return NewResult(response);
        }

        [HttpGet(Router.AuthorizationRouting.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetRoleByIdQuery() { Id = id });
            return NewResult(response);
        }

        // ?? TODO: Fix Status Codes In Swagger Doc Appears as 0
        [ProducesResponseType(typeof(Response<GetRolesForUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
        [HttpGet(Router.AuthorizationRouting.GetUserRoles)] // Get All Roles But For This User Roles They Have a HasRole = True..
        public async Task<IActionResult> GetUserRoles([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetRolesForUserQuery() { UserId = id });
            return NewResult(response);
        }

        [HttpPut(Router.AuthorizationRouting.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromRoute] int id, [FromBody] UpdateUserRolesCommand command)
        {
            command.UserId = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        #endregion

        #region Claim Endpoints

        [HttpGet(Router.AuthorizationRouting.GetUserClaims)]
        public async Task<IActionResult> GetUserClaims([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetClaimsForUserQuery() { UserId = id });
            return NewResult(response);
        }



        #endregion

    }


}
