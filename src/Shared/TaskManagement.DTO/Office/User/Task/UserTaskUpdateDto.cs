namespace TaskManagement.DTO.Office.User.Task;

public record UserTaskUpdateDto {
    public required Guid Id { get; init; }

    public Guid? UserId { get; init; }

    public DateTime? DueDate { get; init; }

    public string? Title { get; init; }
    public string? Description { get; init; }

    public Guid? LinkedObjectId { get; init; }
}