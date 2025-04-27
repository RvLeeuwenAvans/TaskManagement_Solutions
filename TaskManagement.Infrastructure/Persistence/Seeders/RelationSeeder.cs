using TaskManagement.Domain.Office.Relation;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class RelationSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.Relations.Any())
        {
            var office = context.Offices.First(); // Ensure an office exists
            var relations = new List<Relation>
            {
                new() { FirstName = "Alice", LastName = "Johnson", OfficeId = office.Id, Office = office },
                new() { FirstName = "Bob", LastName = "Brown", OfficeId = office.Id, Office = office }
            };

            await context.Relations.AddRangeAsync(relations);
            await context.SaveChangesAsync();
        }
    }
}