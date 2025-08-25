using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public partial class Department : BaseEntity
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmentSubject>();
        }

        [StringLength(150)]
        public string NameEn { get; set; }

        [StringLength(150)]
        public string NameAr { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }
    }
}
