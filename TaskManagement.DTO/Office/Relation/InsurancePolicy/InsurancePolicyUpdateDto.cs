namespace TaskManagement.DTO.Office.Relation.InsurancePolicy;

public record InsurancePolicyUpdateDto
{
    public Guid Id { get; init; }

    public string? Type { get; init; }
}