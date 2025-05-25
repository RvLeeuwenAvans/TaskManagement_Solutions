using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.DTO.Office.User.Task;

public record UserTaskResponse {
    public required Guid Id { get; init; }

    public required UserResponse User { get; init; }

    public required DateTime DueDate { get; init; }

    public required string CreatorName { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }

    public LinkedObjectResponseDto? LinkedObject { get; set; }
}