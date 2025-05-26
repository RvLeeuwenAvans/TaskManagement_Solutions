namespace TaskManagement.MobileApp.Models;

public enum LinkedObjectType
{
    Relation,
    DamageClaim,
    InsurancePolicy
}

public record LinkedObjectModel(
    LinkedObjectType Type,
    string Name);