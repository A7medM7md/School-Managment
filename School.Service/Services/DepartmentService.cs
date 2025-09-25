using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Data.Entities.Procedures;
using School.Data.Entities.Views;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Abstracts.Procedures;
using School.Infrastructure.Abstracts.Views;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IViewRepository<DepartmentStudentsCountView> _departmentStudentsCountViewRepository;
        private readonly IGetStudentsCountByDepartmentIdProcedureRepository _getCountByDeptIdProcRepository;

        public DepartmentService(IDepartmentRepository departmentRepository,
                                    IViewRepository<DepartmentStudentsCountView> departmentStudentsCountViewRepository,
                                    IGetStudentsCountByDepartmentIdProcedureRepository getCountByDeptIdProcRepository
        )
        {
            _departmentRepository = departmentRepository;
            _departmentStudentsCountViewRepository = departmentStudentsCountViewRepository;
            _getCountByDeptIdProcRepository = getCountByDeptIdProcRepository;
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository
                .GetTableNoTracking()
                .Include(D => D.Instructors)
                //.Include(D => D.Students) [Fetched From Student Service To Apply Pagination On Students List]
                .Include(D => D.DepartmentSubjects).ThenInclude(S => S.Subject)
                .Include(D => D.Manager)
                .SingleOrDefaultAsync(D => D.Id == id); // If There Is a Duplicated Id [Will Throw Exception] => Never Happens
        }

        public async Task<bool> IsDepartmentWithIdExists(int id)
        {
            return await _departmentRepository.GetTableNoTracking().AnyAsync(D => D.Id == id);
        }


        public IQueryable<DepartmentStudentsCountView> GetDepartmentStudentsCount()
        {
            return _departmentStudentsCountViewRepository
                .GetTableNoTracking();
        }

        public async Task<IReadOnlyList<GetStudentsCountByDepartmentIdProcedure>> GetStudentsCountByDepartmentIdAsync(GetStudentsCountByDepartmentIdProcedureParams procedureParams)
        {
            return await _getCountByDeptIdProcRepository.GetStudentsCountByDepartmentIdAsync(procedureParams);
        }
    }
}
