using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Note.Validators;

public class NoteUpdateDtoValidator : AbstractValidator<UpdateNote>
{
    public NoteUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(255);
    }
}