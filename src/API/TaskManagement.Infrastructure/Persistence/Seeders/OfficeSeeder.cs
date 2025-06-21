using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class OfficeSeeder(TaskManagementDatabaseContext context, ILogger<OfficeSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 1;
    public override string Name => "Offices";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<Office>()) 
        {
            Logger.LogInformation("Offices already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding offices...");

        var offices = new List<Office>
        {
            new() { Name = "Neunen" },
            new() { Name = "Eindhoven" }
        };

        await Context.Offices.AddRangeAsync(offices);
        await Context.SaveChangesAsync();
        
        Logger.LogInformation("Seeded {Count} offices", offices.Count);
    }
}