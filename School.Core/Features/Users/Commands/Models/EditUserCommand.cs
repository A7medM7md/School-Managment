using MediatR;
using School.Core.Bases;
using System.Text.Json.Serialization;

namespace School.Core.Features.Users.Commands.Models
{
    public class EditUserCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
