using MediatR;
using School.Data.Commons;
using School.Data.Helpers.Email;

namespace School.Core.Features.Emails.Commands.Models
{
    public class SendEmailCommand : IRequest<Response<string>>
    {
        public string ToEmail { get; set; }
        public EmailContent Content { get; set; } = new EmailContent();
    }
}
