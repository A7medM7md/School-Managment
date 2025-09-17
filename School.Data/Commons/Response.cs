﻿using Microsoft.AspNetCore.Http;

namespace School.Data.Commons
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; }

        public object Meta { get; set; } = null!;

        public T Data { get; set; } = default!;

        // Private constructor to force using factory methods
        public Response() { }

        // Success factory
        public static Response<T> Success(
            T data,
            string message = "Request completed successfully",
            int statusCode = StatusCodes.Status200OK,
            object meta = null)
        {
            return new Response<T>
            {
                Succeeded = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = null,
                Meta = meta
            };
        }

        // Fail factory (with list of errors)
        public static Response<T> Fail(
            string message = "Request failed",
            int statusCode = StatusCodes.Status400BadRequest,
            List<string> errors = null,
            object meta = null)
        {
            return new Response<T>
            {
                Succeeded = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>(),
                Data = default,
                Meta = meta
            };
        }

        // Fail factory (single error)
        public static Response<T> Fail(
            string error,
            int statusCode = StatusCodes.Status400BadRequest)
        {
            return Fail("Request failed", statusCode, new List<string> { error });
        }
    }
}
