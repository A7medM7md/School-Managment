using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities.Views;

namespace School.Infrastructure.Configurations
{
    internal class DepartmentStudentsCountViewConfigurations : IEntityTypeConfiguration<DepartmentStudentsCountView>
    {
        public void Configure(EntityTypeBuilder<DepartmentStudentsCountView> builder)
        {
            // View read-only
            builder.HasNoKey();

            builder.ToView("DeptStdCount");

            builder.Property(e => e.DepartmentId).HasColumnName("Id");
            builder.Property(e => e.StudentsCount).HasColumnName("Students Count");

        }
    }
}
