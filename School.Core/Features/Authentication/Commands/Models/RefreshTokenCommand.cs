using MediatR;
using School.Data.Commons;
using School.Data.Helpers.JWT;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<SignInResponse>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
