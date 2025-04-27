using TaskManagement.Domain.Office.User.Task.LinkedObject;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class LinkedObjectSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.LinkedObjects.Any())
        {
            var linkedObjects = new List<LinkedObject>
            {
                new() { UserTask = context.Tasks.First(), Relation = context.Relations.First() },
                new() { UserTask = context.Tasks.First(), DamageClaim = context.DamageClaims.First() }
            };

            await context.LinkedObjects.AddRangeAsync(linkedObjects);
            await context.SaveChangesAsync();
        }
    }
}