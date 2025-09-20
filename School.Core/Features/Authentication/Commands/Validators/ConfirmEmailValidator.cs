using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authentication.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Authentication.Commands.Validators
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;


        public ConfirmEmailValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }


        private void ApplyValidationsRules()
        {
            RuleFor(U => U.UserId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(U => U.Token)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

    }
}
