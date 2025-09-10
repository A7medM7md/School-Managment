using MediatR;
using School.Core.Bases;
using School.Data.Helpers.JWT;
using System.ComponentModel.DataAnnotations;

namespace School.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<Response<SignInResponse>>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
