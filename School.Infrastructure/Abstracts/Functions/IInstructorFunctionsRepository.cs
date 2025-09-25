using School.Data.Entities;
using School.Infrastructure.Bases;

namespace School.Infrastructure.Abstracts.Functions
{
    public interface IInstructorFunctionsRepository : ISqlRepository
    {
        public Task<decimal> GetInstructorsSalarySummation();
        public Task<List<Instructor>> GetInstructorsData();
    }
}