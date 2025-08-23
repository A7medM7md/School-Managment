using MediatR;
using School.Core.Bases;
using System.Text.Json.Serialization;

namespace School.Core.Features.Students.Commands.Models
{
    public class EditStudentCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string? Phone { get; set; }
        public int DepartmentId { get; set; }
    }
}
