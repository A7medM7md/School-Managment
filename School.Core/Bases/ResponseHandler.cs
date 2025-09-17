using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using School.Core.Resources;
using School.Data.Commons;

namespace School.Core.Bases
{
    public class ResponseHandler
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ResponseHandler(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        // Success (200)
        public Response<T> Success<T>(T data, string? message = null, object meta = null) =>
            Response<T>.Success(
                data,
                message ?? _localizer[SharedResourcesKeys.Success],
                StatusCodes.Status200OK,
                meta
            );

        // Created (201)
        public Response<T> Created<T>(T data, string? message = null, object meta = null) =>
            Response<T>.Success(
                data,
                message ?? _localizer[SharedResourcesKeys.Created],
                StatusCodes.Status201Created,
                meta
            );

        // Deleted (200 / 204)
        public Response<T> Deleted<T>(string? message = null, bool noContent = false) =>
            Response<T>.Success(
                default,
                message ?? _localizer[SharedResourcesKeys.Deleted],
                noContent ? StatusCodes.Status204NoContent : StatusCodes.Status200OK
            );

        // BadRequest (400)
        public Response<T> BadRequest<T>(string? message = null, List<string>? errors = null) =>
            Response<T>.Fail(
                message ?? _localizer[SharedResourcesKeys.BadRequest],
                StatusCodes.Status400BadRequest,
                errors
            );

        // Unauthorized (401)
        public Response<T> Unauthorized<T>(string? message = null) =>
            Response<T>.Fail(
                message ?? _localizer[SharedResourcesKeys.UnAuthorized],
                StatusCodes.Status401Unauthorized
            );

        // NotFound (404)
        public Response<T> NotFound<T>(string? message = null) =>
            Response<T>.Fail(
                message ?? _localizer[SharedResourcesKeys.NotFound],
                StatusCodes.Status404NotFound
            );

        // UnprocessableEntity (422)
        public Response<T> UnprocessableEntity<T>(string? message = null, List<string>? errors = null) =>
            Response<T>.Fail(
                message ?? _localizer[SharedResourcesKeys.UnprocessableEntity],
                StatusCodes.Status422UnprocessableEntity,
                errors
            );
    }
}
