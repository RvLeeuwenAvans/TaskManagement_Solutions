namespace TaskManagement.DTO.Office.User.Task;

public record CreateUserTask {
    public required Guid UserId { get; init; }

    public required DateTime DueDate { get; init; }
    
    public required string CreatorName { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }

    public Guid? LinkedObjectId { get; init; }
}