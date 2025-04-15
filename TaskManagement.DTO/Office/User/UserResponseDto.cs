using System;

namespace TaskManagement.DTO.Office.User;

public record UserResponseDto
{
    public Guid Id { get; init; }
    public Guid OfficeId { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}