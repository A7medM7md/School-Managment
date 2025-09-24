using School.Data.Entities;
using School.Data.Entities.Views;

namespace School.Service.Abstracts
{
    public interface IDepartmentService
    {
        public Task<Department?> GetDepartmentByIdAsync(int id);
        public Task<bool> IsDepartmentWithIdExists(int id);

        // View Service
        public IQueryable<DepartmentStudentsCountView> GetDepartmentStudentsCount();

    }
}
