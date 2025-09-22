using Microsoft.AspNetCore.Identity;
using School.Data.Commons;
using School.Data.Entities.Identity;
using School.Data.Helpers.Email;
using School.Infrastructure.Context;
using School.Service.Abstracts;
using System.Security.Cryptography;

namespace School.Service.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public PasswordResetService(UserManager<AppUser> userManager,
            IEmailService emailService,
            ApplicationDbContext Context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = Context;
        }

        public async Task<Response<string>> GenerateAndSendResetCodeAsync(string email, CancellationToken cancellationToken)
        {
            // Get User
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Response<string>.Fail("user not found", 404);

            // Generate Code
            var code = GenerateOTP(6);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Update User Reset Code In DB
                user.ResetCode = code; // Stored Hashed Automatically
                user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
                var updateResult = await _userManager.UpdateAsync(user);

                // Send Code To User Email

                var emailContent = new EmailContent
                {
                    Subject = "Reset your password",
                    RecipientName = user.FullName ?? user.UserName ?? "User",
                    LeadText = "You requested a password reset.",
                    BodyText = $@"Your password reset code is: <strong>{code}</strong>
                                <br> It is valid for 15 minutes."
                };

                // Send Confirmation Email
                var sendResult = await _emailService.SendEmailAsync(email, emailContent, cancellationToken);

                await transaction.CommitAsync();

                return Response<string>.Success("Reset code sent to email");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Response<string>.Fail("Failed to generate reset code", 500, new List<string>() { ex.Message });
            }
        }


        public Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            throw new NotImplementedException();
        }


        private string GenerateOTP(int length)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            var result = "";
            foreach (var b in bytes)
                result += (b % 10).ToString();

            return result;
        }

    }
}
