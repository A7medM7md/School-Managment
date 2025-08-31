using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Departments.Queries.Models;
using School.Core.Features.Departments.Queries.Responses;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
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
    }
}
