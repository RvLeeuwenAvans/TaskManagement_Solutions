using System;

namespace TaskManagement.DTO.Office.User;

public record UserResponse {
    public Guid Id { get; init; }
    public Guid OfficeId { get; set; }
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public required string Email { get; set; }
}