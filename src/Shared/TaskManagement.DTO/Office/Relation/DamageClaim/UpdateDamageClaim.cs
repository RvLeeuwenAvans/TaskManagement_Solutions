namespace TaskManagement.DTO.Office.Relation.DamageClaim;

public record UpdateDamageClaim
{
    public Guid Id { get; init; }

    public required string Type { get; init; }
}