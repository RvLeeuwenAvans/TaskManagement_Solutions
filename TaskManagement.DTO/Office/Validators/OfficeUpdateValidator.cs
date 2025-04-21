using FluentValidation;

namespace TaskManagement.DTO.Office.Validators;

public class OfficeUpdateDtoValidator : AbstractValidator<OfficeUpdateDto>
{
    public OfficeUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}