using Microsoft.AspNetCore.Http;
using School.Data.Entities;

namespace School.Service.Abstracts
{
    public interface IInstructorService
    {
        public Task<decimal> GetTotalSalary();
        public Task<List<Instructor>> GetAllInstructors();
        public Task<bool> IsNameExists(string name);
        public Task<string> AddInstructorAsync(Instructor instructor, IFormFile image);

    }
}
