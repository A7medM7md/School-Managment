using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Core.Bases;

namespace School.Api.Base
{
    [ApiController]
    public class AppBaseController : ControllerBase
    {
        private IMediator? _mediator; // Cache or Store IMediator Service
        // Property Injection
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        #region Actions

        public ObjectResult NewResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case StatusCodes.Status200OK:
                    return new OkObjectResult(response);
                case StatusCodes.Status201Created:
                    return new CreatedResult(string.Empty, response);
                case StatusCodes.Status401Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case StatusCodes.Status400BadRequest:
                    return new BadRequestObjectResult(response);
                case StatusCodes.Status404NotFound:
                    return new NotFoundObjectResult(response);
                case StatusCodes.Status202Accepted:
                    return new AcceptedResult(string.Empty, response);
                case StatusCodes.Status422UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }

        #endregion

    }
}
