using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Users.Commands.Models;
using School.Core.Features.Users.Queries.Models;
using School.Core.Features.Users.Queries.Responses;
using School.Core.Wrappers;
using School.Data.AppMetaData;
using School.Data.Commons;

namespace School.Api.Controllers
{
    [Authorize]
    public class UsersController : AppBaseController
    {
        [HttpPost(Router.UserRouting.Create)] // POST: api/v1/users
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<string>>> Create([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.UserRouting.PaginatedList)]  // GET: api/v1/users/paginated?
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResult<GetPaginatedUsersResponse>>> GetAllPaginated([FromQuery] GetPaginatedUsersQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }


        [HttpGet(Router.UserRouting.GetById)]  // GET: api/v1/users/{id}
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Response<GetUserByIdResponse>>> Get([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetUserByIdQuery(id));
            return NewResult(response);
        }

        [HttpPut(Router.UserRouting.Update)]  // PUT: api/v1/users/{id}
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<string>>> Edit([FromRoute] int id, [FromBody] EditUserCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.UserRouting.Delete)] // DELETE: api/v1/users/{id}
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<string>>> Delete([FromRoute] int id)
            => NewResult(await Mediator.Send(new DeleteUserCommand(id)));

        [HttpPut(Router.UserRouting.ChangePassword)]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Response<string>>> ChangePassword([FromRoute] int id, [FromBody] ChangeUserPasswordCommand command)
        {
            command.Id = id;
            return NewResult(await Mediator.Send(command));
        }

    }
}
