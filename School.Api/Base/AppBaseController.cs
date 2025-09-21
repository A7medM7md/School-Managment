using MediatR;
using Microsoft.AspNetCore.Mvc;
using School.Data.Commons;

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
                // Success
                case StatusCodes.Status200OK:
                    return new OkObjectResult(response);
                case StatusCodes.Status201Created:
                    return new CreatedResult(string.Empty, response);
                case StatusCodes.Status202Accepted:
                    return new AcceptedResult(string.Empty, response);
                case StatusCodes.Status203NonAuthoritative:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status203NonAuthoritative };
                case StatusCodes.Status204NoContent:
                    return new ObjectResult(null) { StatusCode = StatusCodes.Status204NoContent };
                case StatusCodes.Status205ResetContent:
                    return new ObjectResult(null) { StatusCode = StatusCodes.Status205ResetContent };
                case StatusCodes.Status206PartialContent:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status206PartialContent };
                case StatusCodes.Status207MultiStatus:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status207MultiStatus };
                case StatusCodes.Status208AlreadyReported:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status208AlreadyReported };
                case StatusCodes.Status226IMUsed:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status226IMUsed };

                // Redirection
                case StatusCodes.Status300MultipleChoices:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status300MultipleChoices };
                case StatusCodes.Status301MovedPermanently:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status301MovedPermanently };
                case StatusCodes.Status302Found:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status302Found };
                case StatusCodes.Status303SeeOther:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status303SeeOther };
                case StatusCodes.Status304NotModified:
                    return new ObjectResult(null) { StatusCode = StatusCodes.Status304NotModified };
                case StatusCodes.Status305UseProxy:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status305UseProxy };
                case StatusCodes.Status307TemporaryRedirect:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status307TemporaryRedirect };
                case StatusCodes.Status308PermanentRedirect:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status308PermanentRedirect };

                // Client errors
                case StatusCodes.Status400BadRequest:
                    return new BadRequestObjectResult(response);
                case StatusCodes.Status401Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case StatusCodes.Status402PaymentRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status402PaymentRequired };
                case StatusCodes.Status403Forbidden:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden };
                case StatusCodes.Status404NotFound:
                    return new NotFoundObjectResult(response);
                case StatusCodes.Status405MethodNotAllowed:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status405MethodNotAllowed };
                case StatusCodes.Status406NotAcceptable:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status406NotAcceptable };
                case StatusCodes.Status407ProxyAuthenticationRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status407ProxyAuthenticationRequired };
                case StatusCodes.Status408RequestTimeout:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status408RequestTimeout };
                case StatusCodes.Status409Conflict:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status409Conflict };
                case StatusCodes.Status410Gone:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status410Gone };
                case StatusCodes.Status411LengthRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status411LengthRequired };
                case StatusCodes.Status412PreconditionFailed:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status412PreconditionFailed };
                case StatusCodes.Status413PayloadTooLarge:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status413PayloadTooLarge };
                case StatusCodes.Status414RequestUriTooLong:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status414RequestUriTooLong };
                case StatusCodes.Status415UnsupportedMediaType:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status415UnsupportedMediaType };
                case StatusCodes.Status416RangeNotSatisfiable:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status416RangeNotSatisfiable };
                case StatusCodes.Status417ExpectationFailed:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status417ExpectationFailed };
                case StatusCodes.Status418ImATeapot:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status418ImATeapot };
                case StatusCodes.Status421MisdirectedRequest:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status421MisdirectedRequest };
                case StatusCodes.Status422UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                case StatusCodes.Status423Locked:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status423Locked };
                case StatusCodes.Status424FailedDependency:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status424FailedDependency };
                case StatusCodes.Status426UpgradeRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status426UpgradeRequired };
                case StatusCodes.Status428PreconditionRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status428PreconditionRequired };
                case StatusCodes.Status429TooManyRequests:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status429TooManyRequests };
                case StatusCodes.Status431RequestHeaderFieldsTooLarge:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge };
                case StatusCodes.Status451UnavailableForLegalReasons:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status451UnavailableForLegalReasons };

                // Server errors
                case StatusCodes.Status500InternalServerError:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                case StatusCodes.Status501NotImplemented:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status501NotImplemented };
                case StatusCodes.Status502BadGateway:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status502BadGateway };
                case StatusCodes.Status503ServiceUnavailable:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status503ServiceUnavailable };
                case StatusCodes.Status504GatewayTimeout:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status504GatewayTimeout };
                case StatusCodes.Status505HttpVersionNotsupported:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status505HttpVersionNotsupported };
                case StatusCodes.Status506VariantAlsoNegotiates:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status506VariantAlsoNegotiates };
                case StatusCodes.Status507InsufficientStorage:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status507InsufficientStorage };
                case StatusCodes.Status508LoopDetected:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status508LoopDetected };
                case StatusCodes.Status510NotExtended:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status510NotExtended };
                case StatusCodes.Status511NetworkAuthenticationRequired:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status511NetworkAuthenticationRequired };

                // Fallback
                default:
                    return new ObjectResult(response) { StatusCode = response.StatusCode };
            }
        }

        #endregion

    }
}
