using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities;

namespace School.Infrastructure.Configurations
{
    public class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasOne(D => D.Manager)
                .WithOne(I => I.ManagedDepartment)
                .HasForeignKey<Department>(D => D.InsManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(D => D.Instructors)
                .WithOne(I => I.Department)
                .HasForeignKey(I => I.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(D => D.NameAr)
                .HasMaxLength(150);

            builder.Property(D => D.NameEn)
                .HasMaxLength(150);
        }
    }
}
