using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Emails.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Emails.Commands.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public SendEmailValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        #endregion


        #region Handle Functions

        private void ApplyValidationsRules()
        {

            RuleFor(E => E.ToEmail)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.InvalidEmail]);

            RuleFor(E => E.Subject)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(E => E.Message)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

        #endregion

    }
}
