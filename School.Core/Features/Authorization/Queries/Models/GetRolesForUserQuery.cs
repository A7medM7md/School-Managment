using MediatR;
using School.Core.Bases;
using School.Core.Features.Authorization.Queries.Responses;

namespace School.Core.Features.Authorization.Queries.Models
{
    public class GetRolesForUserQuery : IRequest<Response<GetRolesForUserResponse>>
    {
        public int UserId { get; set; }
    }
}
