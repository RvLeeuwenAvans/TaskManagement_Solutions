using Microsoft.Extensions.Hosting;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        await OfficeSeeder.SeedAsync(context);          // No dependencies
        await UserSeeder.SeedAsync(context);            // Depends on Office
        await RelationSeeder.SeedAsync(context);        // Depends on Office
        await DamageClaimSeeder.SeedAsync(context);     // Depends on Relation
        await InsurancePolicySeeder.SeedAsync(context); // Depends on Relation
        await UserTaskSeeder.SeedAsync(context);        // Depends on User
        await NoteSeeder.SeedAsync(context);            // Depends on UserTask
        await LinkedObjectSeeder.SeedAsync(context);    // Depends on UserTask, Relation, and DamageClaim
    }
}