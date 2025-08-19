using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Infrastructure.Bases
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();

        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();


        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
    }
}
