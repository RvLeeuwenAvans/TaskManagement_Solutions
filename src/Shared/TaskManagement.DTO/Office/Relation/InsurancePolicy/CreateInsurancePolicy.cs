namespace TaskManagement.DTO.Office.Relation.InsurancePolicy;

public record CreateInsurancePolicy
{
    public Guid RelationId { get; init; }
    
    public required string Type { get; init; }
}