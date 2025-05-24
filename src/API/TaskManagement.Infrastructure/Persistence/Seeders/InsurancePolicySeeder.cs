using TaskManagement.Domain.Office.Relation.InsurancePolicy;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class InsurancePolicySeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.InsurancePolicies.Any())
        {
            var relation = context.Relations.First();
            var insurancePolicies = new List<InsurancePolicy>
            {
                new() { Type = "Health", RelationId = relation.Id, Relation = relation },
                new() { Type = "Car", RelationId = relation.Id, Relation = relation }
            };

            await context.InsurancePolicies.AddRangeAsync(insurancePolicies);
            await context.SaveChangesAsync();
        }
    }
}