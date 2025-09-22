using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace School.Data.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Country { get; set; }
        [EncryptColumn]
        public string? ResetCode { get; set; } // Hashed
        public DateTime? ResetCodeExpiry { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();

    }
}
