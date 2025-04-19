using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.DamageClaim.Validators;

public class DamageClaimUpdateDtoValidator : AbstractValidator<DamageClaimUpdateDto>
{
    public DamageClaimUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("DamageClaim ID is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(100).WithMessage("Type cannot exceed 100 characters.");
    }
}