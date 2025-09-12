using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Authentication.Commands.Models;
using School.Data.AppMetaData;
using School.Data.Helpers.JWT;
using School.Service.Responses;

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


        [HttpPost(Router.AuthRouting.RefreshToken)]
        public async Task<ActionResult<Response<SignInResponse>>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        [HttpPost(Router.AuthRouting.ValidateToken)]
        public async Task<ActionResult<Response<TokenValidationResponse>>> ValidateToken([FromBody] ValidateTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
