namespace TaskManagement.DTO.Office.User.Task.Note;

public record NoteUpdateDto {
    public Guid Id { get; init; }

    public required string Content { get; init; }
}