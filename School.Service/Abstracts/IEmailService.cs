using School.Data.Commons;

namespace School.Service.Abstracts
{
    public interface IEmailService
    {
        public Task<Response<string>> SendEmailAsync(string toEmail, string subject, string message, CancellationToken cancellationToken);
    }
}
