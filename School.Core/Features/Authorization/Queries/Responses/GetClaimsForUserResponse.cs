using School.Data.Dtos;

namespace School.Core.Features.Authorization.Queries.Responses
{
    public class GetClaimsForUserResponse
    {
        public int UserId { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }
}
