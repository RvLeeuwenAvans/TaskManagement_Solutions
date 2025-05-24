using FluentValidation;

namespace TaskManagement.DTO.Office.User.Task.Note.Validators;

public class NoteCreateDtoValidator : AbstractValidator<NoteCreateDto>
{
    public NoteCreateDtoValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(255);
    }
}