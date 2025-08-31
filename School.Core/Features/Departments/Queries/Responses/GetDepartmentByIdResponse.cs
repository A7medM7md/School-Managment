using School.Core.Wrappers;

namespace School.Core.Features.Departments.Queries.Responses
{
    // DTO
    public class GetDepartmentByIdResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ManagerName { get; set; }

        public PaginatedResult<StudentResponse>? Students { get; set; }
        public List<SubjectResponse> Subjects { get; set; } = new();
        public List<InstructorResponse> Instructors { get; set; } = new();
    }

    // Sub DTOs
    public class StudentResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public StudentResponse(int id, string? name)
        {
            Id = id;
            Name = name;
        }
    }

    public class SubjectResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class InstructorResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
