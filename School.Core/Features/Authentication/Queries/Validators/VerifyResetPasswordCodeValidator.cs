using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Queries.Models
{
    public class VerifyResetPasswordCodeValidator : AbstractValidator<VerifyResetPasswordCodeQuery>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;


        public VerifyResetPasswordCodeValidator(IStringLocalizer<SharedResources> localizer)
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

        }
    }
}
