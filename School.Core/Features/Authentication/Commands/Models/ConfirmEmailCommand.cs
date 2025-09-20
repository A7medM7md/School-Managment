using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class ConfirmEmailCommand : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
