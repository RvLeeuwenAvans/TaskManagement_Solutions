namespace TaskManagement.DTO.Office;

public record OfficeResponse {
    public Guid Id { get; init; }
    
    public required string Name { get; set; }
    public required int OfficeCode { get; set; }
}