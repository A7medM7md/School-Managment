using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository
                .GetTableNoTracking()
                .Include(D => D.Instructors)
                .Include(D => D.Students)
                .Include(D => D.DepartmentSubjects).ThenInclude(S => S.Subject)
                .Include(D => D.Manager)
                .SingleOrDefaultAsync(D => D.Id == id); // If There Is a Duplicated Id [Will Throw Exception] => Never Happens
        }
    }
}
