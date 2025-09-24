using Microsoft.EntityFrameworkCore;
using School.Data.Entities.Views;
using School.Infrastructure.Abstracts.Views;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories.Views
{
    internal class DepartmentStudentsCountViewRepository : GenericRepositoryAsync<DepartmentStudentsCountView>, IViewRepository<DepartmentStudentsCountView>
    {
        private readonly DbSet<DepartmentStudentsCountView> _departmentStudentsCountView;

        public DepartmentStudentsCountViewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _departmentStudentsCountView = dbContext.Set<DepartmentStudentsCountView>();
        }
    }
}
