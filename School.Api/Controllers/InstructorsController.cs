using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Features.Instructors.Queries.Models;
using School.Core.Features.Instructors.Queries.Responses;
using School.Data.AppMetaData;
using School.Data.Commons;

namespace School.Api.Controllers
{
    public class InstructorsController : AppBaseController
    {
        [HttpGet(Router.InstructorRouting.GetTotalSalary)]
        public async Task<ActionResult<Response<decimal>>> GetInstructorsSalarySummation()
        {
            var response = await Mediator.Send(new GetInstructorsSalarySummationQuery());
            return NewResult(response);
        }

        [HttpGet(Router.InstructorRouting.GetAll)]
        public async Task<ActionResult<Response<List<GetInstructorsDataResponse>>>> GetInstructorsData()
        {
            var response = await Mediator.Send(new GetInstructorsDataQuery());
            return NewResult(response);
        }

        [HttpPost(Router.InstructorRouting.Add)]
        public async Task<IActionResult> AddInstructor([FromForm] AddInstructorCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
