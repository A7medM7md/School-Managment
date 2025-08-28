using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities;

namespace School.Infrastructure.Configurations
{
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(S => S.NameEn).HasMaxLength(150);
            builder.Property(S => S.NameAr).HasMaxLength(150);
            builder.Property(S => S.Address).HasMaxLength(250);
            builder.Property(S => S.Phone).HasMaxLength(20);
        }
    }
}
