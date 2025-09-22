using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Commands.Validators
{
    public class SendResetPasswordCodeValidator : AbstractValidator<SendResetPasswordCodeCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;


        public SendResetPasswordCodeValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }


        private void ApplyValidationsRules()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.InvalidEmail])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

    }
}
