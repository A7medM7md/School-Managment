using FluentValidation;
using School.Core.Features.Students.Commands.Models;
using School.Core.Resources;
using School.Service.Abstracts;

namespace School.Core.Features.Students.Commands.Validators
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {

        #region Fields
        private readonly IStudentService _studentService;

        #endregion

        #region Constructors
        public AddStudentValidator(IStudentService studentService)
        {
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
            _studentService = studentService;
        }

        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(S => S.NameEn)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty) // null + empty + whitespace
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .MaximumLength(25).WithMessage("Name cannot be greater than 25 characters.")
                .MinimumLength(2).WithMessage("'{PropertyValue}' is invalid name, cannot be less than 2 characters.");

            RuleFor(S => S.Address)
                .NotEmpty().WithMessage(SharedResourcesKeys.NotEmpty)
                .NotNull().WithMessage(SharedResourcesKeys.Required)
                .Matches("^\\d+\\s[A-Za-z0-9\\s,.'-]+$") // 123 Main Street
                .WithMessage("{PropertyName} must start with a number then street name, e.g., '123 Main Street'."); ;
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(S => S.NameEn)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameExists(Key))
                .WithMessage(SharedResourcesKeys.IsExist);
        }



        #endregion

    }
}
