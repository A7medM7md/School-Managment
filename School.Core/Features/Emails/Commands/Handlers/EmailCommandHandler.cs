using MediatR;
using Microsoft.Extensions.Localization;
using School.Core.Bases;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Resources;
using School.Data.Commons;
using School.Service.Abstracts;

namespace School.Core.Features.Emails.Commands.Handlers
{
    public class EmailCommandHandler : ResponseHandler,
                                            IRequestHandler<SendEmailCommand, Response<string>>
    {
        private readonly IEmailService _emailService;

        public EmailCommandHandler(IEmailService emailService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _emailService = emailService;
        }

        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Message, cancellationToken);


            if (!response.Succeeded)
            {
                return Response<string>.Fail(
                    message: response.Message,
                    statusCode: response.StatusCode,
                    errors: response.Errors
                );
            }

            return Success(response.Message);
        }
    }
}
