using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Users.Commands.Models;
using School.Core.Resources;

namespace School.Core.Features.Users.Commands.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {

        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion


        #region Constructors

        public AddUserValidator(IStringLocalizer<SharedResources> localizer)
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

            RuleFor(U => U.Password)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MinimumLength(8).WithMessage(_localizer[SharedResourcesKeys.MinLengthis8]);

            RuleFor(U => U.ConfirmPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .Equal(U => U.Password).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);
        }


        private void ApplyCustomValidationsRules()
        {

        }

        #endregion

    }
}
