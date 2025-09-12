using MediatR;
using School.Core.Bases;
using School.Service.Responses;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class ValidateTokenCommand : IRequest<Response<TokenValidationResponse>>
    {
        public string AccessToken { get; set; }
    }
}
