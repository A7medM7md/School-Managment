using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Emails.Commands.Models
{
    public class SendEmailCommand : IRequest<Response<string>>
    {
        public string ToEmail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; } = "Reset Password";
    }
}
