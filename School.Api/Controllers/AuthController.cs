using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Authentication.Commands.Models;
using School.Data.AppMetaData;
using School.Data.Helpers.JWT;

namespace School.Api.Controllers
{
    public class AuthController : AppBaseController
    {
        [HttpPost(Router.AuthRouting.SignIn)]
        public async Task<ActionResult<Response<SignInResponse>>> SignIn([FromForm] SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
