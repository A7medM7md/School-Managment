using MediatR;
using School.Core.Features.Instructors.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Instructors.Queries.Models
{
    public class GetInstructorsDataQuery : IRequest<Response<List<GetInstructorsDataResponse>>>
    {
    }
}
