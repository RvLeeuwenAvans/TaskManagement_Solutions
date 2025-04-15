using System;

namespace TaskManagement.DTO.Office.User.Task;

public record UserTaskCreateDto
{
    public Guid UserId { get; init; }
    
    public required string Title { get; init; }
    public string? Description { get; init; }
    
    public Guid? LinkedObjectId { get; init; }
}