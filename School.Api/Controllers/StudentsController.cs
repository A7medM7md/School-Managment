using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Core.Features.Students.Queries.Models;

namespace School.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Students/List")]  // GET: Url/api/Students/List
        public async Task<IActionResult> GetStudentsList()
        {
            var response = await _mediator.Send(new GetStudentsListQuery());

            return Ok(response);
        }
        
        [HttpGet("/Students/{id}")] // GET: Url/api/Students/1
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetStudentByIdQuery(id));

            return Ok(response);
        }

    }
}
