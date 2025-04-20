using System;
using TaskManagement.Domain.Office;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.Domain.Office.Relation.DamageClaim;

namespace TaskManagement.Test.ServiceTests.Helpers;

public static class TestHelpers
{
    public static Office CreateTestOffice(Guid? id = null, string name = "Test Office")
    {
        return new Office
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            OfficeCode = 1000
        };
    }

    public static Relation CreateTestRelation(Guid? id = null, string? firstName = null, string? lastName = null)
    {
        return new Relation
        {
            Id = id ?? Guid.NewGuid(),
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            OfficeId = Guid.NewGuid(),
            RelationNumber = 42,
            Office = CreateTestOffice()
        };
    }

    public static DamageClaim CreateTestDamageClaim(
        Guid? id = null,
        string type = "Water Damage",
        int damageNumber = 123,
        int damageNumberSub = 1,
        Relation? relation = null)
    {
        relation ??= CreateTestRelation();

        return new DamageClaim
        {
            Id = id ?? Guid.NewGuid(),
            Type = type,
            DamageNumber = damageNumber,
            DamageNumberSub = damageNumberSub,
            RelationId = relation.Id,
            Relation = relation
        };
    }
}