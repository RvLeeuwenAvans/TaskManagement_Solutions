using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.Validators;

public class RelationUpdateDtoValidator : AbstractValidator<UpdateRelation>
{
    public RelationUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Relation ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");
    }
}