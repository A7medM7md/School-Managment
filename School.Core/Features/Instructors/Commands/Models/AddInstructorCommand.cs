using MediatR;
using Microsoft.AspNetCore.Http;
using School.Data.Commons;

namespace School.Core.Features.Instructors.Commands.Models
{
    public class AddInstructorCommand : IRequest<Response<string>>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public IFormFile Image { get; set; }

        public int DepartmentId { get; set; }
        public int SupervisorId { get; set; }

    }
}
