using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Commands.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;


        public ResetPasswordValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }


        private void ApplyValidationsRules()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.InvalidEmail])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(u => u.ResetCode)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(u => u.NewPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

    }
}
