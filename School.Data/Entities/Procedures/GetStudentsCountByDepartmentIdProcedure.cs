using School.Data.Commons;

namespace School.Data.Entities.Procedures
{
    public class GetStudentsCountByDepartmentIdProcedure : ILocalizableEntity
    {
        public int DepartmentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int StudentsCount { get; set; }
    }

    public class GetStudentsCountByDepartmentIdProcedureParams
    {
        public int DeptId { get; set; } = 0;
    }
}
