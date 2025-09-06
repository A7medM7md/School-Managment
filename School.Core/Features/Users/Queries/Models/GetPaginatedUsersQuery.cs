using MediatR;
using School.Core.Features.Users.Queries.Responses;
using School.Core.Wrappers;

namespace School.Core.Features.Users.Queries.Models
{
    public class GetPaginatedUsersQuery : IRequest<PaginatedResult<GetPaginatedUsersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
