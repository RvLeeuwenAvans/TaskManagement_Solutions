namespace TaskManagement.DTO.Office.Relation.InsurancePolicy;

public record InsurancePolicyResponseDto
{
    public Guid Id { get; init; }
    public Guid RelationId { get; init; }

    public int PolicyNumber { get; init; }
    
    public required string Type { get; init; }
}