using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Validators;

public class UserTaskCreateDtoValidator : AbstractValidator<UserTaskCreateDto>
{
    public UserTaskCreateDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .When(x => x.Description is not null);
    }
}