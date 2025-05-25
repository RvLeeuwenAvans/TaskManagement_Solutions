namespace TaskManagement.DTO.Office;

public record UpdateOffice {
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
}