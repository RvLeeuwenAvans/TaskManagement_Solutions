using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.LinkedObject.Validators;

public class LinkedObjectCreateDtoValidator : AbstractValidator<LinkedObjectCreateDto>
{
    public LinkedObjectCreateDtoValidator()
    {
        RuleFor(x => x.UserTaskId).NotEmpty().WithMessage("UserTaskId is required.");
        RuleFor(x => x)
            .Must(dto =>
                dto.RelationId != null ||
                dto.DamageClaimId != null ||
                dto.InsurancePolicyId != null)
            .WithMessage("One linked relation, damage claim, or insurance policy must be provided.");
    }
}
