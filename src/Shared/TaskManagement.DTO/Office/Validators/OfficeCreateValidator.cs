using FluentValidation;
using TaskManagement.DTO.Office;

namespace TaskManagement.DTO.Office.Validators;

public class OfficeCreateDtoValidator : AbstractValidator<OfficeCreateDto>
{
    public OfficeCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}