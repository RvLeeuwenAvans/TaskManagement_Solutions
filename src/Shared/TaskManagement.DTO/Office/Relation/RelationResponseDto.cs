namespace TaskManagement.DTO.Office.Relation;

public record RelationResponseDto
{
    public Guid Id { get; init; }
    public Guid OfficeId { get; init; }
    
    public int RelationNumber { get; init; }
    
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}