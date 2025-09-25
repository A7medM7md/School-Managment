using School.Data.Entities.Procedures;
using School.Infrastructure.Abstracts.Procedures;
using School.Infrastructure.Context;
using StoredProcedureEFCore;

namespace School.Infrastructure.Repositories.Procedures
{
    public class GetStudentsCountByDepartmentIdProcedureRepository : IGetStudentsCountByDepartmentIdProcedureRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GetStudentsCountByDepartmentIdProcedureRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyList<GetStudentsCountByDepartmentIdProcedure>>
        GetStudentsCountByDepartmentIdAsync(GetStudentsCountByDepartmentIdProcedureParams procedureParams)
        {
            var result = new List<GetStudentsCountByDepartmentIdProcedure>();

            await _dbContext.LoadStoredProc("GetStudentsCountByDepartmentId")
                .AddParam("DeptId", procedureParams.DeptId)
                .ExecAsync(async reader =>
                {
                    result = await reader.ToListAsync<GetStudentsCountByDepartmentIdProcedure>();
                });

            return result.AsReadOnly();
        }
    }
}
