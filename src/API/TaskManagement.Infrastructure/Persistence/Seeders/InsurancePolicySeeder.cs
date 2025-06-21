using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class InsurancePolicySeeder(TaskManagementDatabaseContext context, ILogger<InsurancePolicySeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 5;
    public override string Name => "Insurance Policies";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<InsurancePolicy>()) 
        {
            Logger.LogInformation("Insurance policies already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding insurance policies...");

        var relation = await Context.Relations.FirstAsync(r => r.FirstName == "Jolijn");
        var insurancePolicies = new List<InsurancePolicy>
        {
            new() { Type = "Inboedelverzekering", RelationId = relation.Id, Relation = relation },
            new() { Type = "Opstalverzekering", RelationId = relation.Id, Relation = relation }
        };

        await Context.InsurancePolicies.AddRangeAsync(insurancePolicies);
        await Context.SaveChangesAsync();
        
        Logger.LogInformation("Seeded {Count} insurance policies", insurancePolicies.Count);
    }
}