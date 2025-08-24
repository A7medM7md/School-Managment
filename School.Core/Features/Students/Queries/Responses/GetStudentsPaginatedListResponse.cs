namespace School.Core.Features.Students.Queries.Responses
{
    public class GetStudentsPaginatedListResponse
    {
        public GetStudentsPaginatedListResponse(int id, string name, string address, string departmentName)
        {
            StudID = id;
            Name = name;
            Address = address;
            DepartmentName = departmentName;
        }

        public int StudID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? DepartmentName { get; set; }
    }
}
