using MediatR;
using School.Core.Bases;
using School.Data.Dtos;
using System.Text.Json.Serialization;

namespace School.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserRolesCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public IReadOnlyList<RoleDto>? Roles { get; set; }
    }
}
