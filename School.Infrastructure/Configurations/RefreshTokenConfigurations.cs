using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Data.Entities.Identity;

namespace School.Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(rt => rt.JwtId)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(rt => rt.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(rt => rt.Token)
                   .IsUnique();

            builder.HasIndex(rt => rt.UserId);

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
