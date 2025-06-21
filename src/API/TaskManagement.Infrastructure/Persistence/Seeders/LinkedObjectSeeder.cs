using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.User.Task.LinkedObject;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class LinkedObjectSeeder(TaskManagementDatabaseContext context, ILogger<LinkedObjectSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 8;
    public override string Name => "Linked Objects";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<LinkedObject>())
        {
            Logger.LogInformation("Linked objects already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding linked objects...");

        var linkedObjects = new List<LinkedObject>
        {
            new()
            {
                UserTask = await Context.Tasks.FirstAsync(t => t.Title == "Onderhoud doorvoeren"),
                Relation = await Context.Relations.OrderBy(r => r.Id).FirstAsync()
            },
            new()
            {
                UserTask = await Context.Tasks.FirstAsync(t => t.Title == "Documenten doornemen"),
                DamageClaim = await Context.DamageClaims.OrderBy(d => d.Id).FirstAsync()
            }
        };

        await Context.LinkedObjects.AddRangeAsync(linkedObjects);
        await Context.SaveChangesAsync();

        // Update the back-references
        var firstTask = await Context.Tasks.OrderBy(t => t.Id).FirstAsync();
        var lastTask = await Context.Tasks.OrderByDescending(t => t.Id).FirstAsync();

        firstTask.LinkedObjectId = linkedObjects.First().Id;
        lastTask.LinkedObjectId = linkedObjects.Last().Id;

        await Context.SaveChangesAsync();

        Logger.LogInformation("Seeded {Count} linked objects", linkedObjects.Count);
    }
}