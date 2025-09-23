using School.Data.Commons;

namespace School.Service.Abstracts
{
    public interface IPasswordResetService
    {
        public Task<Response<string>> GenerateAndSendResetCodeAsync(string email, CancellationToken cancellationToken);
        public Task<Response<bool>> VerifyResetCodeAsync(string email, string code);
        public Task<Response<bool>> ResetPasswordAsync(string email, string code, string newPassword);

    }
}