using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities;

namespace School.Infrastructure.Configurations
{
    public class InstructorConfigurations : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasOne(I => I.Supervisor)
                .WithMany(I => I.Instructors)
                .HasForeignKey(I => I.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(I => I.Salary).HasColumnType("decimal(18,2)");
            builder.Property(I => I.NameAr).HasMaxLength(150);
            builder.Property(I => I.NameEn).HasMaxLength(150);
            builder.Property(I => I.Address).HasMaxLength(250);
            builder.Property(I => I.Position).HasMaxLength(50);
        }
    }
}
