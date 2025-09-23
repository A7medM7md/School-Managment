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

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

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

                await transaction.CommitAsync(cancellationToken);

                return Response<string>.Success("Reset code sent to email");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Response<string>.Fail(ex.Message, "Failed to generate reset code", 500);
            }
        }

        public async Task<Response<bool>> VerifyResetCodeAsync(string email, string code)
        {
            var (_, error) = await ValidateResetCodeAsync(email, code);
            if (error != null) return error;
            return Response<bool>.Success(true);
        }

        public async Task<Response<bool>> ResetPasswordAsync(string email, string code, string newPassword)
        {
            var (user, error) = await ValidateResetCodeAsync(email, code); // Validate Again For Security
            if (error != null) return error;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Remove old password
                var removeResult = await _userManager.RemovePasswordAsync(user);
                // Add new password
                var addResult = await _userManager.AddPasswordAsync(user, newPassword);

                // Clear OTP for one-time usage
                user.ResetCode = null;
                user.ResetCodeExpiry = null;
                await _userManager.UpdateAsync(user);

                await transaction.CommitAsync();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Response<bool>.Fail(ex.Message, "Failed to reset password", 500);
            }
        }


        // Shared validation logic
        private async Task<(AppUser user, Response<bool> error)> ValidateResetCodeAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return (null!, Response<bool>.Fail("User not found", 404));

            if (user.ResetCodeExpiry < DateTime.UtcNow)
            {
                user.ResetCode = null;
                user.ResetCodeExpiry = null;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                    return (null!, Response<bool>.Fail(
                        "Failed to clear expired reset code",
                        500,
                        updateResult.Errors.Select(e => e.Description).ToList()
                    ));

                return (null!, Response<bool>.Fail("Reset code expired", 400));
            }

            if (user.ResetCode != code)
                return (null!, Response<bool>.Fail("Invalid reset code", 400));

            return (user, null!);
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
