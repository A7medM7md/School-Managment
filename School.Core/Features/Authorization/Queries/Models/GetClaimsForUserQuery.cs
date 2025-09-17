using MediatR;
using School.Core.Features.Authorization.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Authorization.Queries.Models
{
    public class GetClaimsForUserQuery : IRequest<Response<GetClaimsForUserResponse>>
    {
        public int UserId { get; set; }
    }
}
