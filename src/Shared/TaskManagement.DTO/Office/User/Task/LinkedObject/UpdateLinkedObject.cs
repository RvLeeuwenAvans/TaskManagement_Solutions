namespace TaskManagement.DTO.Office.User.Task.LinkedObject;

public record UpdateLinkedObject
{
    public Guid Id { get; init; }
    public Guid? RelationId { get; init; }
    public Guid? DamageClaimId { get; init; }
    public Guid? InsurancePolicyId { get; init; }
}