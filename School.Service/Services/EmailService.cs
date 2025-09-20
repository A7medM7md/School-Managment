using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using School.Data.Commons;
using School.Data.Helpers.Email;
using School.Service.Abstracts;
using System.Net;
using System.Security.Authentication;

namespace School.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task<Response<string>> SendEmailAsync(string toEmail, string subject, string message, CancellationToken cancellationToken)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("School System", _emailSettings.From));
                mimeMessage.To.Add(new MailboxAddress("", toEmail));
                mimeMessage.Subject = subject;

                #region If You Need To Use Html Template

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "email.html");


                if (!File.Exists(templatePath))
                    return Response<string>.Fail("Email template not found.", 500);

                var htmlTemplate = File.ReadAllText(templatePath);
                htmlTemplate = htmlTemplate
                    .Replace("{{LEAD_TEXT}}", "This is a notification from School System.")
                    .Replace("{{MESSAGE_PLACEHOLDER}}", WebUtility.HtmlEncode(message))
                    .Replace("{{CTA_LINK}}", "https://your-app.example.com/details/123")
                    .Replace("{{CTA_TEXT}}", "View Details")
                    .Replace("{{NOTIF_ID}}", DateTime.UtcNow.Ticks.ToString());

                #endregion

                mimeMessage.Body = new BodyBuilder
                {
                    TextBody = message,
                    //HtmlBody = $"<p>{message}</p>"
                    HtmlBody = htmlTemplate
                }.ToMessageBody();

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_emailSettings.SmtpServer,
                    _emailSettings.Port,
                    MailKit.Security.SecureSocketOptions.StartTls,
                    cancellationToken);

                await smtp.AuthenticateAsync(_emailSettings.UserName,
                    _emailSettings.Password,
                    cancellationToken);

                await smtp.SendAsync(mimeMessage, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                return Response<string>.Success("Email sent successfully");
            }
            catch (SmtpCommandException ex) when (ex.ErrorCode == SmtpErrorCode.RecipientNotAccepted)
            {
                return Response<string>.Fail($"Invalid recipient address: {toEmail}", 400);
            }
            catch (AuthenticationException)
            {
                return Response<string>.Fail("Authentication failed: invalid username or password", 401);
            }
            catch (ServiceNotConnectedException)
            {
                return Response<string>.Fail("SMTP server is not reachable", 503);
            }
            catch (ServiceNotAuthenticatedException)
            {
                return Response<string>.Fail("SMTP authentication required", 401);
            }
            catch (Exception ex)
            {
                return Response<string>.Fail($"Error sending email: {ex.Message}", 500);
            }
        }


    }
}
