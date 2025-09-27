using School.Data.Commons;

namespace School.Data.Entities
{
    public class Instructor : BaseEntity, ILocalizableEntity
    {
        public Instructor()
        {
            Instructors = new HashSet<Instructor>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }

        //[StringLength(150)]
        public string? NameAr { get; set; }
        //[StringLength(150)]
        public string? NameEn { get; set; }
        //[StringLength(250)]
        public string? Address { get; set; }
        //[StringLength(50)]
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
        public string? Image { get; set; }

        // Self => 1 : M
        public int? SupervisorId { get; set; }
        //[InverseProperty(nameof(Instructors))]
        public virtual Instructor? Supervisor { get; set; }

        // Self => M : 1
        //[InverseProperty(nameof(Supervisor))]
        public virtual ICollection<Instructor> Instructors { get; set; }

        // Instructor → Department (M:1)
        public int? DepartmentId { get; set; }
        //[InverseProperty(nameof(Department.Instructors))]
        public virtual Department? Department { get; set; }

        // Instructor → ManagedDepartment (1:1)
        //[InverseProperty(nameof(Department.Manager))]
        public virtual Department? ManagedDepartment { get; set; }

        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}
