namespace TaskManagement.MobileApp.Models.Collections;

public enum LinkedObjectType
{
    Relation,
    DamageClaim,
    InsurancePolicy
}

public record LinkedObjectItem(
    Guid Id,
    LinkedObjectType Type,
    string Name);