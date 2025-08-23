using FluentValidation;
using School.Core.Features.Students.Commands.Models;
using School.Service.Abstracts;

namespace School.Core.Features.Students.Commands.Validators
{
    public class EditStudentValidator : AbstractValidator<EditStudentCommand>
    {
        #region Fields
        private readonly IStudentService _studentService;

        #endregion

        #region Constructors
        public EditStudentValidator(IStudentService studentService)
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
                .NotEmpty().WithMessage("Name cannot be null or empty.")
                .MaximumLength(25).WithMessage("Name cannot be greater than 25 characters.")
                .MinimumLength(2).WithMessage("{PropertyValue} is invalid name, {PropertyName} cannot be less than 2 characters.");

            RuleFor(S => S.Address)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .NotNull().WithMessage("{PropertyName} cannot be null.")
                .Matches("^\\d+\\s[A-Za-z0-9\\s,.'-]+$") // 123 Main Street
                .WithMessage("{PropertyName} must start with a number then street name, e.g., '123 Main Street'."); ;
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(S => S.Name)
               .MustAsync(async (model, Key, CancellationToken) => !await _studentService.IsNameExistsExcludeSelf(Key, model.Id))
               .WithMessage("Name is exists");
        }


        #endregion

    }
}
