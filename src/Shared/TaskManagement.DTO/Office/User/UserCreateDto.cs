namespace TaskManagement.DTO.Office.User;

public record UserCreateDto {
    public Guid OfficeId { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    public required string Email { get; init; }
    public required string Password { get; init; }
}