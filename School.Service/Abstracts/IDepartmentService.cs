using School.Data.Entities;

namespace School.Service.Abstracts
{
    public interface IDepartmentService
    {
        public Task<Department?> GetDepartmentByIdAsync(int id);
    }
}
