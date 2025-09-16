using School.Data.Dtos;

namespace School.Core.Features.Authorization.Queries.Responses
{
    public class GetRolesForUserResponse
    {
        public int UserId { get; set; }
        public IReadOnlyList<RoleDto>? Roles { get; set; }
    }
}
