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

        public async Task<Response<string>> SendEmailAsync(string toEmail, EmailContent content, CancellationToken cancellationToken)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("School System", _emailSettings.From));
                mimeMessage.To.Add(new MailboxAddress(content.RecipientName, toEmail));
                mimeMessage.Subject = content.Subject;

                #region Use Html Template For Email

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "email.html");
                if (!File.Exists(templatePath))
                    return Response<string>.Fail("Email template not found.", 500);

                var ctaVisible = content.ActionLink == "#" ? "none" : "inline-block";

                var htmlTemplate = File.ReadAllText(templatePath);
                htmlTemplate = htmlTemplate
                    .Replace("{{USERNAME}}", WebUtility.HtmlEncode(content.RecipientName))
                    .Replace("{{LEAD_TEXT}}", WebUtility.HtmlEncode(content.LeadText))
                    .Replace("{{MESSAGE_PLACEHOLDER}}", content.BodyText)
                    .Replace("{{CTA_LINK}}", content.ActionLink)
                    .Replace("{{CTA_TEXT}}", content.ActionText)
                    .Replace("{{CTA_VISIBLE}}", ctaVisible)
                    .Replace("{{NOTIF_ID}}", DateTime.UtcNow.Ticks.ToString())
                    .Replace("{{YEAR}}", DateTime.UtcNow.Year.ToString());

                #endregion

                mimeMessage.Body = new BodyBuilder
                {
                    TextBody = $"Notification from School System:\n\n{content.BodyText}",
                    //HtmlBody = $"<p>{message}</p>"
                    HtmlBody = htmlTemplate
                }.ToMessageBody();

                using var smtp = new SmtpClient();

                try
                {
                    await smtp.ConnectAsync(_emailSettings.SmtpServer,
                        _emailSettings.Port,
                        MailKit.Security.SecureSocketOptions.StartTls,
                        cancellationToken);

                    await smtp.AuthenticateAsync(_emailSettings.UserName,
                        _emailSettings.Password,
                        cancellationToken);

                    await smtp.SendAsync(mimeMessage, cancellationToken);
                }
                finally
                {
                    if (smtp.IsConnected)
                        await smtp.DisconnectAsync(true, cancellationToken);
                }

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
