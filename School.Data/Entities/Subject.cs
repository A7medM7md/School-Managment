using School.Data.Commons;
using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public class Subject : BaseEntity, ILocalizableEntity
    {
        public Subject()
        {
            StudentSubjects = new HashSet<StudentSubject>();
            DepartmentSubjects = new HashSet<DepartmentSubject>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }

        [StringLength(100)]
        public string? NameEn { get; set; }
        [StringLength(100)]
        public string? NameAr { get; set; }
        public DateTime? Period { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
        public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}
