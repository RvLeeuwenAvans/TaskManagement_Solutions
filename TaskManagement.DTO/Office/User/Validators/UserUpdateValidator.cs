using FluentValidation;

namespace TaskManagement.DTO.Office.User.Validators;

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");

        When(x => x.FirstName is not null, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);
        });

        When(x => x.LastName is not null, () =>
        {
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(50);
        });
    }
}