using FluentValidation;
using School.Core.Features.Students.Commands.Models;
using School.Service.Abstracts;

namespace School.Core.Features.Students.Commands.Validators
{
    public class AddStudentValidator : AbstractValidator<AddStudentCommand>
    {
        private readonly IStudentService _studentService;

        #region Fields

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
            RuleFor(S => S.Name)
                .NotEmpty().WithMessage("Name cannot be empty.") // null + empty + whitespace
                .NotNull().WithMessage("Name cannot be null.")
                .MaximumLength(25).WithMessage("Name cannot be greater than 25 characters.")
                .MinimumLength(2).WithMessage("{PropertyValue} is invalid name, cannot be less than 2 characters.");

            RuleFor(S => S.Address)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .Matches("^\\d+\\s[A-Za-z0-9\\s,.'-]+$") // 123 Main Street
                .WithMessage("{PropertyName} must start with a number then street name, e.g., '123 Main Street'."); ;
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(S => S.Name)
                .MustAsync(async (Key, CancellationToken) => !await _studentService.IsNameExists(Key))
                .WithMessage("Name is exists");
        }


        #endregion

    }
}
