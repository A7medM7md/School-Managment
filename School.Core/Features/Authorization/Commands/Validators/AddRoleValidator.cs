using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Authorization.Commands.Validators
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;

        #endregion


        #region Constructors

        public AddRoleValidator(IStringLocalizer<SharedResources> localizer,
            IAuthorizationService authorizationService)
        {
            _localizer = localizer;
            _authorizationService = authorizationService;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }

        #endregion


        #region Handle Functions

        private void ApplyValidationsRules()
        {
            RuleFor(R => R.RoleName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(R => R.RoleName)
                .MustAsync(async (Key, CancellationToken) => !await _authorizationService.IsRoleExist(Key))
                .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
        }

        #endregion
    }
}
