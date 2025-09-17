using MediatR;
using School.Data.Commons;
using System.Text.Json.Serialization;

namespace School.Core.Features.Authorization.Commands.Models
{
    public class EditRoleCommand : IRequest<Response<string>>
    {
        public EditRoleCommand(int id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }

        [JsonIgnore]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
