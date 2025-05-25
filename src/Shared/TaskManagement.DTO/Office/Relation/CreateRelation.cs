namespace TaskManagement.DTO.Office.Relation;

public record CreateRelation
{
    public Guid OfficeId { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}