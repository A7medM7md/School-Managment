namespace School.Core.Wrappers
{
    public class PaginatedResult<T>
    {
        private PaginatedResult(bool succeeded, List<T>? data = null, List<string>? messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data ?? new List<T>();
            Messages = messages ?? new List<string>();
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Succeeded = succeeded;
        }

        public List<T> Data { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public List<string> Messages { get; set; }

        public bool Succeeded { get; set; }

        public Dictionary<string, object> Meta { get; set; } = new();

        // Factory Methods
        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize) =>
            new(true, data, null, count, page, pageSize);

        public static PaginatedResult<T> Failure(List<string> errors) =>
            new(false, null, errors);
    }
}
