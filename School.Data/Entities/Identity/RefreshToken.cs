namespace School.Data.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpireAt { get; set; }

        public virtual AppUser? User { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireAt;
    }

}
