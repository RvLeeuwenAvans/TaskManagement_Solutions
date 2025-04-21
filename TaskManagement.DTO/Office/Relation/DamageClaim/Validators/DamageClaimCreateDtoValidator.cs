using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.DamageClaim.Validators;

public class DamageClaimCreateDtoValidator : AbstractValidator<DamageClaimCreateDto>
{
    public DamageClaimCreateDtoValidator()
    {
        RuleFor(x => x.RelationId).NotEmpty().WithMessage("RelationId is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(100).WithMessage("Type cannot exceed 100 characters.");
    }
}