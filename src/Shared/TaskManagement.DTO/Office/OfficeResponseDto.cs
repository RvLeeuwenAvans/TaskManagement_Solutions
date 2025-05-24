namespace TaskManagement.DTO.Office;

public record OfficeResponseDto {
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
    public required int OfficeCode { get; init; }
}