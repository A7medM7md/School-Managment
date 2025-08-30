using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {

        #region Fields and Properties

        private readonly DbSet<Department> departments;

        #endregion

        #region Constructors
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            departments = dbContext.Set<Department>();
        }

        #endregion


        #region Handle Functions


        #endregion

    }
}
