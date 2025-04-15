namespace TaskManagement.DTO.Office.User.Task.Note;

public record NoteCreateDto
{
    public Guid TaskId { get; init; }
    
    public required string Content { get; init; }
}