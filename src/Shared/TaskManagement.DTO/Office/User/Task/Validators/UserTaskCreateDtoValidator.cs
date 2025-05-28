using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Validators;

public class UserTaskCreateDtoValidator : AbstractValidator<CreateUserTask>
{
    public UserTaskCreateDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.CreatorName)
            .NotEmpty();

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .When(x => x.Description is not null);
    }
}