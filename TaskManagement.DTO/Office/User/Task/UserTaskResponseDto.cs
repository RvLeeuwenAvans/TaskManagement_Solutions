using System;

namespace TaskManagement.DTO.Office.User.Task;

public record UserTaskResponseDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    
    public required string Title { get; init; }
    public string? Description { get; init; }

    public Guid? LinkedObjectId { get; init; }
}