using MediatR;
using School.Core.Features.Departments.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Departments.Queries.Models
{
    public class GetDepartmentStudentsCountQuery : IRequest<Response<List<GetDepartmentStudentsCountResponse>>>
    {
    }
}
