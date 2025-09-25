using School.Data.Entities;
using School.Infrastructure.Abstracts.Functions;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorFunctionsRepository _instructorFunctionsRepo;

        public InstructorService(IInstructorFunctionsRepository instructorFunctionsRepo)
        {
            _instructorFunctionsRepo = instructorFunctionsRepo;
        }

        public async Task<decimal> GetTotalSalary()
        {
            return await _instructorFunctionsRepo.GetInstructorsSalarySummation();
        }

        public async Task<List<Instructor>> GetAllInstructors()
        {
            return await _instructorFunctionsRepo.GetInstructorsData();
        }
    }
}
