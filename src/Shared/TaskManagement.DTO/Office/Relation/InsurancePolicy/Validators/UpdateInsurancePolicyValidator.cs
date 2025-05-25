using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.InsurancePolicy.Validators;

public class InsurancePolicyUpdateDtoValidator : AbstractValidator<UpdateInsurancePolicy>
{
    public InsurancePolicyUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Insurance Policy ID is required.");

        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Type cannot exceed 100 characters.");
    }
}