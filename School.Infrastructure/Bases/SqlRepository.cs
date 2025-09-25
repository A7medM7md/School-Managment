using Microsoft.EntityFrameworkCore;
using School.Infrastructure.Context;
using System.Data.Common;

namespace School.Infrastructure.Bases
{
    public class SqlRepository : ISqlRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object?> ExecuteScalarAsync(string query)
        {
            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            return await cmd.ExecuteScalarAsync();
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, Func<DbDataReader, T> map)
        {
            var result = new List<T>();

            await using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(map(reader));
            }

            return result;
        }

    }
}
