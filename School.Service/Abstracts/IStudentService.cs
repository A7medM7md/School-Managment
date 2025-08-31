using School.Data.Entities;
using School.Data.Helpers.Enums;
using School.Service.Enums;

namespace School.Service.Abstracts
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentsListAsync();
        public Task<Student?> GetStudentByIdAsync(int id);
        public Task<Student?> GetStudentByIdWithoutDepartmentAsync(int id);
        public Task<string> AddStudentAsync(Student student);
        public Task<string> UpdateStudentAsync(Student student);
        public Task<DeleteStudentResult> DeleteStudentAsync(int id);
        public Task<bool> IsNameExists(string name);
        public Task<bool> IsNameExistsExcludeSelf(string name, int id);

        public IQueryable<Student> GetStudentsQueryable();
        public IQueryable<Student> GetStudentsByDepartmentIdQueryable(int id);

        public IQueryable<Student> GetFilteredStudentsQueryable(StudentOrderBy? orderBy, string? search);

    }
}
