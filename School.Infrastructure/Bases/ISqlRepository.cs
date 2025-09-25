using System.Data.Common;

namespace School.Infrastructure.Bases
{
    public interface ISqlRepository
    {
        public Task<object?> ExecuteScalarAsync(string query);
        public Task<List<T>> ExecuteQueryAsync<T>(string query, Func<DbDataReader, T> map);
    }
}
