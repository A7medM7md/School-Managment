using Microsoft.Extensions.Localization;
using School.Core.Resources;
using System.Net;

namespace School.Core.Bases
{
    public class ResponseHandler
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ResponseHandler(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public Response<T> Deleted<T>(string? message = null, bool noContent = false) =>
            new Response<T>
            {
                StatusCode = noContent ? HttpStatusCode.NoContent : HttpStatusCode.OK,
                Succeeded = true,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.Deleted] : message
            };

        public Response<T> Success<T>(T data, string? message = null, object meta = null) =>
            new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.Success] : message,
                Meta = meta
            };

        public Response<T> Unauthorized<T>(string? message = null, T? data = default) =>
            new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.UnAuthorized] : message
            };

        public Response<T> BadRequest<T>(string? message = null) =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.BadRequest] : message
            };

        public Response<T> UnprocessableEntity<T>(string? message = null) =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.UnprocessableEntity] : message
            };

        public Response<T> NotFound<T>(string? message = null) =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.NotFound] : message
            };

        public Response<T> Created<T>(T data, string? message = null, object meta = null) =>
            new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = message is null ? _stringLocalizer[SharedResourcesKeys.Created] : message,
                Meta = meta
            };
    }
}
