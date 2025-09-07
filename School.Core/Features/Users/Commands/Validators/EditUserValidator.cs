using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Users.Commands.Validators
{
    public class EditUserValidator : AbstractValidator<EditUserCommand>
    {

        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public EditUserValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }

        #endregion


        #region Handle Functions

        private void ApplyValidationsRules()
        {
            RuleFor(U => U.FullName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100])
                .MinimumLength(3).WithMessage(_localizer[SharedResourcesKeys.MinLengthis3]);

            RuleFor(U => U.Email)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .EmailAddress().WithMessage(_localizer[SharedResourcesKeys.InvalidEmail]);

        }


        private void ApplyCustomValidationsRules()
        {

        }

        #endregion

    }
}
