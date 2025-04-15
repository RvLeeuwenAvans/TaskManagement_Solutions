namespace TaskManagement.DTO.Office.Relation;

public record RelationUpdateDto
{
    public Guid Id { get; init; }
    
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}