using School.Data.Entities.Procedures;

namespace School.Infrastructure.Abstracts.Procedures
{
    public interface IGetStudentsCountByDepartmentIdProcedureRepository
    {
        public Task<IReadOnlyList<GetStudentsCountByDepartmentIdProcedure>> GetStudentsCountByDepartmentIdAsync(GetStudentsCountByDepartmentIdProcedureParams procedureParams);
    }
}
