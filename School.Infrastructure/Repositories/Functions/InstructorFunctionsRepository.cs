using School.Data.Entities;
using School.Infrastructure.Abstracts.Functions;
using School.Infrastructure.Bases;
using School.Infrastructure.Context;

namespace School.Infrastructure.Repositories.Functions
{
    public class InstructorFunctionsRepository : SqlRepository, IInstructorFunctionsRepository
    {
        public InstructorFunctionsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<decimal> GetInstructorsSalarySummation()
        {
            var result = await ExecuteScalarAsync("SELECT dbo.GetInstructorsSalarySummation()");
            return Convert.ToDecimal(result);
        }

        public async Task<List<Instructor>> GetInstructorsData()
        {
            var result = await ExecuteQueryAsync(
                "SELECT * FROM dbo.GetInstructorsData()",
                reader => new Instructor
                {
                    // 0 1 2 3 4 5 [Order Of Columns]
                    Id = reader.GetInt32(0),
                    NameAr = reader.GetString(1),
                    NameEn = reader.GetString(2),
                    Address = reader.GetString(3),
                    Position = reader.GetString(4),
                    Salary = reader.GetDecimal(5)
                });

            return result;
        }

    }
}
