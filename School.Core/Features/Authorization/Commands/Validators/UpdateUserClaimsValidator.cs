using FluentValidation;
using Microsoft.Extensions.Localization;
using School.Core.Features.Authorization.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Authorization.Commands.Validators
{
    public class UpdateUserClaimsValidator : AbstractValidator<UpdateUserClaimsCommand>
    {
        #region Fields

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuthorizationService _authorizationService;

        #endregion


        #region Constructors

        public UpdateUserClaimsValidator(IStringLocalizer<SharedResources> localizer,
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
            // UserId
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .NotEmpty()
                .WithMessage("User Id cannot be empty.");

            // Claims collection not null or empty
            RuleFor(x => x.Claims)
                .NotNull().WithMessage("Claims list is required.")
                .Must(r => r.Any()).WithMessage("Claims list cannot be empty.");

            // Validate each RoleDto inside the list
            //RuleForEach(x => x.Claims!).ChildRules(role =>
            //{
            //    role.RuleFor(r => r.RoleId)
            //        .GreaterThan(0)
            //        .WithMessage("RoleId must be greater than 0.");

            //    role.RuleFor(r => r.RoleName)
            //        .NotEmpty().WithMessage("RoleName is required.");

            //    role.RuleFor(r => r.RoleName)
            //        .MaximumLength(100).WithMessage("RoleName cannot exceed 100 characters.");
            //});
        }

        public void ApplyCustomValidationsRules()
        {
            //RuleForEach(x => x.Claims!).ChildRules(role =>
            //{
            //    role.RuleFor(r => r.RoleName)
            //        .MustAsync(async (name, cancellationToken) =>
            //            await _authorizationService.IsRoleExist(name))
            //        .WithMessage("Role does not exist in the system.");
            //});
        }

        #endregion
    }
}
