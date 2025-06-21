using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.Relation;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class RelationSeeder(TaskManagementDatabaseContext context, ILogger<RelationSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 3;
    public override string Name => "Relations";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<Relation>()) 
        {
            Logger.LogInformation("Relations already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding relations...");

        var office = await Context.Offices.FirstAsync(o => o.Name == "Veldsink Eindhoven");
        var relations = new List<Relation>
        {
            new() { FirstName = "Henk", LastName = "Bosma", OfficeId = office.Id, Office = office },
            new() { FirstName = "Jolijn", LastName = "van Vliet", OfficeId = office.Id, Office = office }
        };

        await Context.Relations.AddRangeAsync(relations);
        await Context.SaveChangesAsync();
        
        Logger.LogInformation("Seeded {Count} relations", relations.Count);
    }
}