using Moq;
using TaskManagement.Domain.Office;
using TaskManagement.Domain.Office.Relation;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;
using TaskManagement.Domain.Office.User;
using TaskManagement.Domain.Office.User.Task;
using TaskManagement.Domain.Office.User.Task.Note;

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

    public static InsurancePolicy CreateTestInsurancePolicy(
        Guid? id = null,
        string type = "Car Insurance",
        int policyNumber = 12345,
        Relation? relation = null)
    {
        relation ??= CreateTestRelation();

        return new InsurancePolicy
        {
            Id = id ?? Guid.NewGuid(),
            Type = type,
            PolicyNumber = policyNumber,
            RelationId = relation.Id,
            Relation = relation
        };
    }

    public static Note CreateTestNote(
        Guid? id = null,
        string content = "Test Note",
        Guid? taskId = null)
    {
        var testTask = CreateTestUserTask(taskId);

        return new Note
        {
            Id = id ?? Guid.NewGuid(),
            Content = content,
            TaskId = testTask.Id,
            UserTask = testTask
        };
    }

    public static UserTask CreateTestUserTask(
        Guid? id = null,
        string title = "Test Task",
        string? description = null,
        Guid? userId = null)
    {
        var testUser = CreateTestUser(userId);

        return new UserTask
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            Description = description,
            UserId = testUser.Id,
            User = testUser
        };
    }

    public static User CreateTestUser(
        Guid? id = null,
        string? firstName = null,
        string? lastName = null)
    {
        var testOffice = CreateTestOffice();

        return new User
        {
            Id = id ?? Guid.NewGuid(),
            FirstName = firstName ?? "Test",
            LastName = lastName ?? "User",
            OfficeId = testOffice.Id,
            Office = testOffice
        };
    }
}