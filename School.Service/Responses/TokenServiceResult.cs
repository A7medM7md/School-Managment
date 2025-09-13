namespace School.Service.Responses
{
    public class TokenServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public static TokenServiceResult<T> Ok(T data) =>
            new TokenServiceResult<T> { Success = true, Data = data };

        public static TokenServiceResult<T> Fail(string errorMessage, int? errorCode = null, T? data = default) =>
            new TokenServiceResult<T> { Success = false, ErrorMessage = errorMessage, ErrorCode = errorCode, Data = data };
    }
}
