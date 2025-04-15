namespace TaskManagement.DTO.Office.User;

public record UserUpdateDto
{
    public Guid Id { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}