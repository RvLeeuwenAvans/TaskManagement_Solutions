using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Validators;

public class UserTaskUpdateDtoValidator : AbstractValidator<UpdateUserTask>
{
    public UserTaskUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        When(x => x.UserId.HasValue, () =>
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
        });

        When(x => x.DueDate.HasValue, () =>
        {
            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Due date must be in the future. Or Today.");
        });

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

        When(x => x.LinkedObjectId.HasValue, () =>
        {
            RuleFor(x => x.LinkedObjectId)
                .NotEqual(Guid.Empty);
        });
    }
}