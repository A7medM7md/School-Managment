using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using School.Data.Commons;
using System.Text.Json;

namespace School.Core.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                Response<string> responseModel;

                //TODO:: cover all validation errors
                switch (error)
                {
                    case UnauthorizedAccessException e:
                        // custom application error
                        responseModel = Response<string>.Fail(
                            e.Message,
                            StatusCodes.Status401Unauthorized
                        );
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;

                    case ValidationException e:
                        // custom validation error
                        responseModel = Response<string>.Fail(
                            e.Message,
                            StatusCodes.Status422UnprocessableEntity
                        );
                        response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        responseModel = Response<string>.Fail(
                            e.Message,
                            StatusCodes.Status404NotFound
                        );
                        response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case DbUpdateException e:
                        // can't update error
                        responseModel = Response<string>.Fail(
                            e.Message,
                            StatusCodes.Status400BadRequest
                        );
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    default:
                        // generic unhandled exception
                        var msg = error.Message + (error.InnerException != null ? "\n" + error.InnerException.Message : "");
                        responseModel = Response<string>.Fail(
                            msg,
                            StatusCodes.Status500InternalServerError
                        );
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }

}
