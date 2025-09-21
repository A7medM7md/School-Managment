using School.Data.Commons;
using School.Data.Helpers.Email;

namespace School.Service.Abstracts
{
    public interface IEmailService
    {
        public Task<Response<string>> SendEmailAsync(string toEmail, EmailContent content, CancellationToken cancellationToken);
    }
}
