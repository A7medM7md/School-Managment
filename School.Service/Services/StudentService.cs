using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Service.Abstracts;
using School.Service.Enums;

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

        public async Task<DeleteStudentResult> DeleteStudentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student is null) return DeleteStudentResult.NotFound;

            var trans = _studentRepository.BeginTransaction();

            try
            {
                await _studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                return DeleteStudentResult.Success;
            }
            catch
            {
                await trans.RollbackAsync();
                return DeleteStudentResult.Failed;
            }
        }

        public async Task<Student?> GetStudentByIdWithoutDepartmentAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student;
        }

        public IQueryable<Student> GetStudentsQueryable()
        {
            return _studentRepository
                .GetTableNoTracking()
                .Include(S => S.Department);
        }

        public IQueryable<Student> GetFilteredStudentsQueryable(string[]? orderBy, string? search)
        {
            var students = GetStudentsQueryable();

            // Apply search
            if (!string.IsNullOrWhiteSpace(search))
            {
                students = students.Where(s => s.Name.Contains(search)
                                             || s.Address.Contains(search)
                                             || s.Department.DName.Contains(search));
            }

            return students;
        }


        #endregion

    }
}
