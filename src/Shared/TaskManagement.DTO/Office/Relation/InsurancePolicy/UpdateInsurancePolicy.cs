namespace TaskManagement.DTO.Office.Relation.InsurancePolicy;

public record UpdateInsurancePolicy
{
    public Guid Id { get; init; }

    public string? Type { get; init; }
}