using Microsoft.EntityFrameworkCore;

namespace School.Core.Wrappers
{
    public static class QueryableExtensions
    {
        // Constants
        private const int DEFAULT_PAGE_SIZE = 5;
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int MAX_PAGE_SIZE = 100;

        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // Validation
            pageNumber = pageNumber <= 0 ? DEFAULT_PAGE_NUMBER : pageNumber;
            pageSize = pageSize <= 0 ? DEFAULT_PAGE_SIZE : pageSize;
            pageSize = Math.Min(pageSize, MAX_PAGE_SIZE);

            int count = await source.CountAsync();

            var items = count > 0
                ? await source.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .AsNoTracking()
                              .ToListAsync()
                : new List<T>();

            return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
        }
    }
}
