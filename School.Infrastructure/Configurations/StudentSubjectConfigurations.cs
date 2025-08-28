using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities;

namespace School.Infrastructure.Configurations
{
    public class StudentSubjectConfigurations : IEntityTypeConfiguration<StudentSubject>
    {
        public void Configure(EntityTypeBuilder<StudentSubject> builder)
        {
            builder.Property(SS => SS.Grade)
             .HasColumnType("decimal(18,2)");

            // Configure Composite Key
            builder.HasKey(DS => new { DS.StudentId, DS.SubjectId });
        }
    }
}
