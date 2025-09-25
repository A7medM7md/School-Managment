using MediatR;
using School.Data.Commons;

namespace School.Core.Features.Instructors.Queries.Models
{
    public class GetInstructorsSalarySummationQuery : IRequest<Response<decimal>>
    {
    }
}
