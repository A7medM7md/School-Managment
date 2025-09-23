using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Authentication.Queries.Models
{
    public class VerifyResetPasswordCodeQuery : IRequest<Response<bool>>
    {
        public string Email { get; set; }
        public string ResetCode { get; set; }
    }
}
