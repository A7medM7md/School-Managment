using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Bases;
using School.Core.Features.Students.Commands.Models;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    public class StudentsController : AppBaseController
    {

        [HttpGet(Router.StudentRouting.List)]  // GET: api/v1/students
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

        [HttpPost(Router.StudentRouting.Create)] // POST: api/v1/students
        public async Task<ActionResult<Response<string>>> Create([FromBody] AddStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.StudentRouting.Update)] // PUT: api/v1/students/{id}
        public async Task<ActionResult<Response<string>>> Edit([FromBody] EditStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
