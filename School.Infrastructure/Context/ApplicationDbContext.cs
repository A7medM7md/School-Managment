using Microsoft.EntityFrameworkCore;
using School.Data.Entities;

namespace School.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Composite Key

            modelBuilder.Entity<DepartmentSubject>()
                .HasKey(DS => new { DS.DepartmentId, DS.SubjectId });

            modelBuilder.Entity<InstructorSubject>()
                .HasKey(DS => new { DS.InstructorId, DS.SubjectId });

            modelBuilder.Entity<StudentSubject>()
                .HasKey(DS => new { DS.StudentId, DS.SubjectId });

            // Configure Self Relationship

            modelBuilder.Entity<Instructor>()
                .HasOne(I => I.Supervisor)
                .WithMany(I => I.Instructors)
                .HasForeignKey(I => I.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Instructor>()
                .Property(I => I.Salary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StudentSubject>()
                .Property(I => I.Grade)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Department>()
                .HasOne(D => D.Manager)
                .WithOne(I => I.ManagedDepartment)
                .HasForeignKey<Department>(D => D.InsManagerId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<DepartmentSubject> DepartmentSubjects { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorSubject> InstructorSubjects { get; set; }
    }
}
