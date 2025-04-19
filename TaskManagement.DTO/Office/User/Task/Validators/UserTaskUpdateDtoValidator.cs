using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Validators;

public class UserTaskUpdateDtoValidator : AbstractValidator<UserTaskUpdateDto>
{
    public UserTaskUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50);
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(255);
        });
    }
}