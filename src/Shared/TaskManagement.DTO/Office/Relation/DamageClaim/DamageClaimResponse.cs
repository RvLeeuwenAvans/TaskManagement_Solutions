namespace TaskManagement.DTO.Office.Relation.DamageClaim;

public record DamageClaimResponse
{
    public Guid Id { get; init; }
    public Guid RelationId { get; init; }
    
    public int DamageNumber { get; init; }
    public int DamageNumberSub { get; init; }

    public required string Type { get; init; }
}