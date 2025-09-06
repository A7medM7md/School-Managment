using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Users.Commands.Models;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    public class UsersController : AppBaseController
    {
        [HttpPost(Router.UserRouting.Create)] // POST: api/v1/users
        public async Task<ActionResult<Response<string>>> Create([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
