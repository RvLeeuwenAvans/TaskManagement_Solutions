using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.LinkedObject.Validators;

public class LinkedObjectUpdateDtoValidator : AbstractValidator<UpdateLinkedObject>
{
    public LinkedObjectUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x)
            .Must(dto =>
                dto.RelationId != null ||
                dto.DamageClaimId != null ||
                dto.InsurancePolicyId != null)
            .WithMessage("One linked relation, damage claim, or insurance policy must be provided.");
    }
}