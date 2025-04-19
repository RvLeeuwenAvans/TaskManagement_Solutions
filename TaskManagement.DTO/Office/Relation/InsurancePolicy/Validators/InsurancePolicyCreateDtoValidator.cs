using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.InsurancePolicy.Validators;

public class InsurancePolicyCreateDtoValidator : AbstractValidator<InsurancePolicyCreateDto>
{
    public InsurancePolicyCreateDtoValidator()
    {
        RuleFor(x => x.RelationId).NotEmpty().WithMessage("RelationId is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(100).WithMessage("Type cannot exceed 100 characters.");
    }
}