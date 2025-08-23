using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class StudentService : IStudentService
    {

        #region Fields
        private readonly IStudentRepository _studentRepository;

        #endregion

        #region Constructors
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        #endregion

        #region Handle Functions
        public async Task<List<Student>> GetStudentsListAsync()
        {
            return await _studentRepository.GetStudentsListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            //return await _studentRepository.GetByIdAsync(id);
            return await _studentRepository.GetTableNoTracking()
                .Include(S => S.Department)
                .Where(S => S.StudID.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<string> AddStudentAsync(Student student)
        {
            await _studentRepository.AddAsync(student);
            return "Succeeded";
        }

        public async Task<bool> IsNameExists(string name)
        {
            // Check If Student Name Exists Or Not [Called By Fluent Validation - Add Student]
            return await _studentRepository
                .GetTableNoTracking()
                .AnyAsync(s => s.Name == name);
        }

        public async Task<bool> IsNameExistsExcludeSelf(string name, int id)
        {
            // Check If Student Name Exists Or Not [Called By Fluent Validation - Edit Student]
            return await _studentRepository
                .GetTableNoTracking()
                .AnyAsync(s => s.Name == name && s.StudID != id);
        }

        public async Task<string> UpdateStudentAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
            return "Succeeded";
        }


        #endregion

    }
}
