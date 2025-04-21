using FluentValidation;

namespace TaskManagement.DTO.Office.Relation.Validators;

public class RelationCreateDtoValidator : AbstractValidator<RelationCreateDto>
{
    public RelationCreateDtoValidator()
    {
        RuleFor(x => x.OfficeId).NotEmpty().WithMessage("OfficeId is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");
    }
}