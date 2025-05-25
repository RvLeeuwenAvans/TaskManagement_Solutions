namespace TaskManagement.DTO.Office.User.Task.LinkedObject;

public record CreateLinkedObject
{
    public Guid UserTaskId { get; init; }
    public Guid? RelationId { get; init; }
    public Guid? DamageClaimId { get; init; }
    public Guid? InsurancePolicyId { get; init; }
}