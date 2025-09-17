using MediatR;
using School.Core.Features.Students.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Students.Queries.Models
{
    public class GetStudentsListQuery : IRequest<Response<List<GetStudentsListResponse>>>
    {

    }
}
