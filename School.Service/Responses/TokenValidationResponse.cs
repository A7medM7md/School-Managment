namespace School.Service.Responses
{
    public class TokenValidationResponse
    {
        public bool IsValid { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public List<ClaimDto>? Claims { get; set; }
        public string? SecurityTokenType { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ClaimDto
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

}
