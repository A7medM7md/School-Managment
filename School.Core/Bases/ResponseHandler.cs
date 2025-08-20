using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Bases
{
    public class ResponseHandler
    {
        public ResponseHandler()
        {

        }

        public Response<T> Deleted<T>(string message = "Deleted Successfully", bool noContent = false) =>
            new Response<T>
            {
                StatusCode = noContent ? HttpStatusCode.NoContent : HttpStatusCode.OK,
                Succeeded = true,
                Message = message
            };

        public Response<T> Success<T>(T data, string message = "Success", object meta = null) =>
            new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message,
                Meta = meta
            };

        public Response<T> Unauthorized<T>(string message = "Unauthorized") =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message
            };

        public Response<T> BadRequest<T>(string message = "Bad Request") =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message
            };

        public Response<T> UnprocessableEntity<T>(string message = "Unprocessable Entity") =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Succeeded = false,
                Message = message
            };

        public Response<T> NotFound<T>(string message = "Not Found") =>
            new Response<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message
            };

        public Response<T> Created<T>(T data, string message = "Created", object meta = null) =>
            new Response<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = message,
                Meta = meta
            };
    }
}
