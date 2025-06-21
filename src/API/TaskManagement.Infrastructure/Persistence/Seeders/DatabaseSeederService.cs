using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class DatabaseSeederService(IServiceProvider serviceProvider, ILogger<DatabaseSeederService> logger)
{
    public async Task SeedAllAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TaskManagementDatabaseContext>();

        // Get all registered seeders and order them
        var seeders = scope.ServiceProvider.GetServices<ISeeder>()
            .OrderBy(s => s.Order)
            .ToList();

        logger.LogInformation("Starting database seeding with {Count} seeders", seeders.Count);

        // Use a transaction to ensure atomicity
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            foreach (var seeder in seeders)
            {
                logger.LogInformation("Running seeder: {SeederName} (Order: {Order})",
                    seeder.Name, seeder.Order);

                await seeder.SeedAsync();
            }

            await transaction.CommitAsync();
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Database seeding failed, transaction rolled back");
            throw;
        }
    }
}