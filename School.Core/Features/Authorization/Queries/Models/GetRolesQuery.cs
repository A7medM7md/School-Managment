using MediatR;
using School.Core.Features.Authorization.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Authorization.Queries.Models
{
    public class GetRolesQuery : IRequest<Response<IReadOnlyList<GetRolesResponse>>>
    {
    }
}
