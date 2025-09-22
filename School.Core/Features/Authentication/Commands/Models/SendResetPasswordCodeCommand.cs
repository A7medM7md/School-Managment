using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SendResetPasswordCodeCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
