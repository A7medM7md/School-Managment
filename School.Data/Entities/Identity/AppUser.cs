using Microsoft.AspNetCore.Identity;

namespace School.Data.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Country { get; set; }
    }
}
