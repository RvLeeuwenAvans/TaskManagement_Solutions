namespace TaskManagement.DTO.Office;

public record OfficeUpdateDto {
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
}