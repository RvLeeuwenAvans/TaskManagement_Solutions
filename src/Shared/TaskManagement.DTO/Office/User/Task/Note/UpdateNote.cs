namespace TaskManagement.DTO.Office.User.Task.Note;

public record UpdateNote {
    public Guid Id { get; init; }

    public required string Content { get; init; }
}