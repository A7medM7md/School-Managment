namespace School.Data.Entities
{
    public class InstructorSubject
    {
        // Composite Key [InstructorId + SubjectId] -> Done Inside OnModelCreating() (Fluent API)
        public int InstructorId { get; set; }
        public int SubjectId { get; set; }

        public Instructor Instructor { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
    }
}
