using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Departments.Queries.Models;
using School.Core.Features.Departments.Queries.Responses;
using School.Data.AppMetaData;
using School.Data.Commons;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class DepartmentsController : AppBaseController
    {
        [HttpGet(Router.DepartmentRouting.GetById)] // GET: api/v1/departments/{id}
        public async Task<ActionResult<Response<GetDepartmentByIdResponse>>> GetDepartmentById([FromRoute] int id, [FromQuery] int studentPageNumber,
            [FromQuery] int studentPageSize)
        {
            var query = new GetDepartmentByIdQuery(id, studentPageNumber, studentPageSize);
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet(Router.DepartmentRouting.GetStdCount)]
        public async Task<ActionResult<Response<GetDepartmentStudentsCountResponse>>> GetDepartmentStudentsCount()
        {
            var response = await Mediator.Send(new GetDepartmentStudentsCountQuery());
            return NewResult(response);
        }

        [HttpGet(Router.DepartmentRouting.GetStdCountByDeptId)]
        public async Task<ActionResult<Response<GetStudentsCountByDepartmentIdResponse>>> GetStudentsCountByDepartmentId([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetStudentsCountByDepartmentIdQuery(id));
            return NewResult(response);
        }
    }
}
