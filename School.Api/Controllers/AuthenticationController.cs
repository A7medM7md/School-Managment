using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Authentication.Commands.Models;
using School.Data.AppMetaData;
using School.Data.Commons;
using School.Data.Helpers.JWT;
using School.Service.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace School.Api.Controllers
{
    public class AuthenticationController : AppBaseController
    {
        [HttpPost(Router.AuthenticationRouting.SignIn)]
        public async Task<ActionResult<Response<SignInResponse>>> SignIn([FromForm] SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        [HttpPost(Router.AuthenticationRouting.RefreshToken)]
        public async Task<ActionResult<Response<SignInResponse>>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        [SwaggerOperation(Summary = "Validate JWT Token", Description = "This endpoint validates a JWT token")]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TokenValidationResponse), StatusCodes.Status404NotFound)]
        [HttpPost(Router.AuthenticationRouting.ValidateToken)]
        public async Task<ActionResult<Response<TokenValidationResponse>>> ValidateToken([FromBody] ValidateTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.AuthenticationRouting.ConfirmEmail)] // GET: baseUrl/api/v1/authentication/confirm-email
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
