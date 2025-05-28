namespace TaskManagement.DTO.Office.Relation.DamageClaim;

public record CreateDamageClaim
{
    public Guid RelationId { get; init; }
    
    public required string Type { get; init; }
}