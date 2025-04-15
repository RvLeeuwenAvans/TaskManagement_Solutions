namespace TaskManagement.DTO.Office.Relation.DamageClaim;

public record DamageClaimCreateDto
{
    public Guid RelationId { get; init; }
    
    public required string Type { get; init; }
}