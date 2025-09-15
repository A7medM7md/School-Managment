using MediatR;
using School.Core.Bases;

namespace School.Core.Features.Authorization.Commands.Models
{
    public class AssignRoleCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string RoleName { get; set; }
    }
}
