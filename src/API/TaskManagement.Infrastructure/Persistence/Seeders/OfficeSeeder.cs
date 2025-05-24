using TaskManagement.Domain.Office;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class OfficeSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.Offices.Any())
        {
            var offices = new List<Office>
            {
                new() { Name = "Neunen" },
                new() { Name = "Eindhoven" }
            };

            await context.Offices.AddRangeAsync(offices);
            await context.SaveChangesAsync();
        }
    }
}