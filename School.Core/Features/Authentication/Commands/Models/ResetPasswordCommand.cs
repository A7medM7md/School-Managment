using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class ResetPasswordCommand : IRequest<Response<bool>>
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
    }
}
