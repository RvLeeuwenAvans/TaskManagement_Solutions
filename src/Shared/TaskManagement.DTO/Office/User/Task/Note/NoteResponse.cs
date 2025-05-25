namespace TaskManagement.DTO.Office.User.Task.Note;

public record NoteResponse {
    public Guid Id { get; init; }
    public Guid TaskId { get; init; }
    
    public required string Content { get; init; }
    
    public DateTime CreatedAt { get; init; }
}