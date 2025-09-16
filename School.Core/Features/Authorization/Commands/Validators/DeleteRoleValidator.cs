using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;

        #endregion


        #region Constructors

        public DeleteRoleValidator(IStringLocalizer<SharedResources> localizer,
            IAuthorizationService authorizationService)
        {
            _localizer = localizer;
            _authorizationService = authorizationService;
            ApplyValidationsRules();
            //ApplyCustomValidationsRules();
        }

        #endregion


        #region Handle Functions

        private void ApplyValidationsRules()
        {
            RuleFor(R => R.Id)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

        public void ApplyCustomValidationsRules()
        {
        }

        #endregion
    }
}
