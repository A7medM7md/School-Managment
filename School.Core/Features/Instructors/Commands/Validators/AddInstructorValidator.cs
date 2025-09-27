using FluentValidation;
using School.Core.Features.Instructors.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Instructors.Commands.Validators
{
    public class AddInstructorValidator : AbstractValidator<AddInstructorCommand>
    {
        private readonly IDepartmentService _departmentService;
        private readonly IInstructorService _instructorService;

        public AddInstructorValidator(IDepartmentService departmentService,
            IInstructorService instructorService)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _departmentService = departmentService;
            _instructorService = instructorService;
        }

        public void ApplyValidationsRules()
        {
            RuleFor(i => i.NameEn)
                .NotEmpty().WithMessage(SharedResourcesKeys.Required);

            RuleFor(i => i.NameEn)
                .NotEmpty().WithMessage(SharedResourcesKeys.Required);

            RuleFor(i => i.DepartmentId)
                .NotEmpty().WithMessage(SharedResourcesKeys.Required);



        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(S => S.NameEn)
              .MustAsync(async (Key, CancellationToken) => !await _instructorService.IsNameExists(Key))
              .WithMessage(SharedResourcesKeys.IsExist);

            RuleFor(S => S.NameAr)
                .MustAsync(async (Key, CancellationToken) => !await _instructorService.IsNameExists(Key))
                .WithMessage(SharedResourcesKeys.IsExist);

            RuleFor(i => i.DepartmentId)
                .MustAsync(async (Key, CancellationToken) => await _departmentService.IsDepartmentWithIdExists(Key))
                .WithMessage(SharedResourcesKeys.IsNotExist);

        }
    }
}
