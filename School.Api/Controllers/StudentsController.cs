using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Students.Commands.Models;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Core.Filters;
using School.Core.Wrappers;
using School.Data.AppMetaData;
using School.Data.Commons;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class StudentsController : AppBaseController
    {
        [HttpGet(Router.StudentRouting.PaginatedList)]  // GET: api/v1/students/paginated?
        public async Task<ActionResult<PaginatedResult<GetStudentsPaginatedListResponse>>> GetStudentsPaginatedList([FromQuery] GetStudentsPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [ServiceFilter(typeof(ValidateUserPermissionsFilter))]
        [HttpGet(Router.StudentRouting.List)]  // GET: api/v1/students/all
        public async Task<ActionResult<Response<List<GetStudentsListResponse>>>> GetStudentsList()
        {
            var response = await Mediator.Send(new GetStudentsListQuery());
            return Ok(response);
        }

        [HttpGet(Router.StudentRouting.GetById)] // GET: api/v1/students/1
        public async Task<ActionResult<Response<GetSingleStudentResponse>>> GetStudent([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetStudentByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Policy = "CreateStudent")]
        [HttpPost(Router.StudentRouting.Create)] // POST: api/v1/students
        public async Task<ActionResult<Response<string>>> Create([FromBody] AddStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "EditStudent")]
        [HttpPut(Router.StudentRouting.Update)] // PUT: api/v1/students/{id}
        public async Task<ActionResult<Response<string>>> Edit([FromRoute] int id, [FromBody] EditStudentCommand command)
        {
            command.Id = id;
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "DeleteStudent")]
        [HttpDelete(Router.StudentRouting.Delete)] // DELETE: api/v1/students/{id}
        public async Task<ActionResult<Response<string>>> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteStudentCommand(id));
            return NewResult(response);
        }

    }
}
