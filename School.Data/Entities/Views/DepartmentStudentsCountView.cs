using School.Data.Commons;

namespace School.Data.Entities.Views
{
    //[Keyless]
    public class DepartmentStudentsCountView : ILocalizableEntity
    {
        public int DepartmentId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int StudentsCount { get; set; }
    }
}
