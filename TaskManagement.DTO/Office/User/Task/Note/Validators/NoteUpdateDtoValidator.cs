using FluentValidation;
using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Application.Validators;

public class NoteUpdateDtoValidator : AbstractValidator<NoteUpdateDto>
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