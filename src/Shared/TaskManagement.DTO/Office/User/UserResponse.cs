using System;

namespace TaskManagement.DTO.Office.User;

public record UserResponse {
    public Guid Id { get; init; }
    public Guid OfficeId { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    public required string Email { get; init; }
}