using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Api.Base;
using School.Core.Features.Emails.Commands.Models;
using School.Data.AppMetaData;

namespace School.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class EmailsController : AppBaseController
    {
        [HttpPost(Router.EmailRouting.Send)]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
