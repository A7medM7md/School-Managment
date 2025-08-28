namespace School.Data.Entities
{
    public class DepartmentSubject
    {
        // Composite Key [DepartmentId + SubjectId] -> Done Inside OnModelCreating() (Fluent API)
        public int DepartmentId { get; set; }
        public int SubjectId { get; set; }

        public virtual Department Department { get; set; } = null!;

        public virtual Subject Subject { get; set; } = null!;
    }
}
