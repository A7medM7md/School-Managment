using School.Data.Commons;

namespace School.Data.Entities
{
    public partial class Department : BaseEntity, ILocalizableEntity
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmentSubject>();
            Instructors = new HashSet<Instructor>();
        }

        //[StringLength(150)]
        public string? NameEn { get; set; }

        //[StringLength(150)]
        public string? NameAr { get; set; }

        // 1 : 1
        public int? InsManagerId { get; set; } // ForeignKey For Manage Relationship
        //[ForeignKey(nameof(InsManagerId))]
        //[InverseProperty(nameof(Instructor.ManagedDepartment))]
        public virtual Instructor? Manager { get; set; }

        // Department → Instructors (1:M)
        //[InverseProperty(nameof(Instructor.Department))]
        public virtual ICollection<Instructor> Instructors { get; set; }

        // Department → Students (1:M)
        public virtual ICollection<Student> Students { get; set; }

        // Department → Subjects (1:M)
        public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }
    }
}
