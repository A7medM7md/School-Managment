using MediatR;
using School.Core.Features.Departments.Queries.Responses;
using School.Data.Commons;

namespace School.Core.Features.Departments.Queries.Models
{
    public class GetStudentsCountByDepartmentIdQuery : IRequest<Response<IReadOnlyList<GetStudentsCountByDepartmentIdResponse>>>
    {
        public int DeptId { get; set; }

        public GetStudentsCountByDepartmentIdQuery(int deptId)
        {
            DeptId = deptId;
        }
    }
}
