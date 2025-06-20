namespace TaskManagement.DTO.Office.User;

public record CreateUser {
    public Guid OfficeId { get; init; }
    
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    
    public required string Email { get; set; }
    public required string Password { get; set; }
}