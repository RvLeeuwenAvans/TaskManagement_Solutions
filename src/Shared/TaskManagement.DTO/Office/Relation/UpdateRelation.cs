namespace TaskManagement.DTO.Office.Relation;

public record UpdateRelation
{
    public Guid Id { get; init; }
    
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}