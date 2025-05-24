namespace TaskManagement.DTO.Office;
// todo: for all DTOs rename; remove suffix DTO; dunno why i did that XD
public record OfficeUpdateDto {
    public Guid Id { get; init; }
    
    public required string Name { get; init; }
}