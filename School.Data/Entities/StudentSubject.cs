namespace School.Data.Entities
{
    public class StudentSubject
    {
        // Composite Key [StudentId + SubjectId] -> Done Inside OnModelCreating() (Fluent API)
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        public decimal? Grade { get; set; }

        public virtual Student Student { get; set; } = null!;

        public virtual Subject Subject { get; set; } = null!;

    }
}
