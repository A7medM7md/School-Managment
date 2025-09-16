using Microsoft.AspNetCore.Identity;

namespace School.Service.Responses
{
    public enum RoleStatus
    {
        Success,
        NotFound,
        AlreadyExists,
        HasUsers,
        Failed
    }

    public class RoleResult
    {
        public RoleStatus Status { get; set; }
        public IdentityResult? IdentityResult { get; set; }
        public IdentityRole<int>? Role { get; set; }
    }
}
