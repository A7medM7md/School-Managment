using MediatR;
using School.Core.Features.Students.Queries.Responses;
using School.Core.Wrappers;

namespace School.Core.Features.Students.Queries.Models
{
    public class GetStudentsPaginatedListQuery : IRequest<PaginatedResult<GetStudentsPaginatedListResponse>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string[]? OrderBy { get; set; }
        public string? Search { get; set; }
    }
}
