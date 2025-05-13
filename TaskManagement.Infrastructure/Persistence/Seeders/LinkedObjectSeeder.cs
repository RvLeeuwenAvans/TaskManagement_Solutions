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
                new()
                {
                    UserTask = context.Tasks.OrderBy(t => t.Id).First(),
                    Relation = context.Relations.OrderBy(r => r.Id).First()
                },
                new()
                {
                    UserTask = context.Tasks.OrderByDescending(t => t.Id).First(),
                    DamageClaim = context.DamageClaims.OrderBy(d => d.Id).First()
                }
            };

            await context.LinkedObjects.AddRangeAsync(linkedObjects);
            await context.SaveChangesAsync();

            context.Tasks.OrderBy(t => t.Id).First().LinkedObjectId = linkedObjects.First().Id;
            context.Tasks.OrderByDescending(t => t.Id).First().LinkedObjectId = linkedObjects.Last().Id;

            await context.SaveChangesAsync();
        }
    }

}