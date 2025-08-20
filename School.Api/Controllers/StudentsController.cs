using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Core.Bases;
using School.Core.Features.Students.Queries.Models;
using School.Core.Features.Students.Queries.Responses;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Router.StudentRouting.List)]  // GET: /api/v1/students/list
        public async Task<ActionResult<Response<List<GetStudentsListResponse>>>> GetStudentsList()
        {
            var response = await _mediator.Send(new GetStudentsListQuery());

            return Ok(response);
        }
        
        [HttpGet(Router.StudentRouting.GetById)] // GET: /api/v1/students/1
        public async Task<ActionResult<Response<GetSingleStudentResponse>>> GetStudent([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetStudentByIdQuery(id));

            return Ok(response);
        }

    }
}
