using MediatR;
using School.Data.Commons;
using System.Text.Json.Serialization;

namespace School.Core.Features.Authorization.Commands.Models
{
    public class AssignRoleCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string RoleName { get; set; }
    }
}
