namespace TaskManagement.DTO.Office.User;

public record UpdateUser {
    public Guid Id { get; init; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    public string? Password { get; set; }
}