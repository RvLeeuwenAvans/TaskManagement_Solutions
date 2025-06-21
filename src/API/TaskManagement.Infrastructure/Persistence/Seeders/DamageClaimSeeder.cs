using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.Relation.DamageClaim;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class DamageClaimSeeder(TaskManagementDatabaseContext context, ILogger<DamageClaimSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 4;
    public override string Name => "Damage Claims";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<DamageClaim>()) 
        {
            Logger.LogInformation("Damage claims already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding damage claims...");

        var relation = await Context.Relations.FirstAsync();
        var damageClaims = new List<DamageClaim>
        {
            new() { Type = "Fire", RelationId = relation.Id, Relation = relation },
            new() { Type = "Flood", RelationId = relation.Id, Relation = relation }
        };

        await Context.DamageClaims.AddRangeAsync(damageClaims);
        await Context.SaveChangesAsync();
        
        Logger.LogInformation("Seeded {Count} damage claims", damageClaims.Count);
    }
}