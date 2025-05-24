using TaskManagement.Domain.Office.Relation.DamageClaim;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class DamageClaimSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.DamageClaims.Any())
        {
            var relation = context.Relations.First();
            var damageClaims = new List<DamageClaim>
            {
                new() { Type = "Fire", RelationId = relation.Id, Relation = relation },
                new() { Type = "Flood", RelationId = relation.Id, Relation = relation }
            };

            await context.DamageClaims.AddRangeAsync(damageClaims);
            await context.SaveChangesAsync();
        }
    }
}