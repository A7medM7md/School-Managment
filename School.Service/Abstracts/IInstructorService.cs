using School.Data.Entities;

namespace School.Service.Abstracts
{
    public interface IInstructorService
    {
        public Task<decimal> GetTotalSalary();
        public Task<List<Instructor>> GetAllInstructors();

    }
}
