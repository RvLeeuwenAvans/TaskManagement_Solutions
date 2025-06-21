using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.User;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class UserSeeder(
    TaskManagementDatabaseContext context,
    ILogger<UserSeeder> logger,
    IPasswordHasher<User> passwordHasher)
    : BaseSeeder(context, logger)
{
    public override int Order => 2;
    public override string Name => "Users";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<User>())
        {
            Logger.LogInformation("Users already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding users...");

        var office = await Context.Offices.FirstAsync(o => o.Name == "Veldsink Eindhoven");
        var adminOffice = await Context.Offices.FirstAsync(o => o.Name == "Hoodkantoor");
        
        var users = new List<User>
        {
            // mobile app users
            new()
            {
                FirstName = "Marianne",
                LastName = "Rodepanne",
                Email = "Marianne.Rodepanne@vcn.com",
                Password = "wachtwoord",
                OfficeId = office.Id,
                Office = office
            },
            // setup an Administrator for the CMS
            new()
            {
                FirstName = "Admin",
                LastName = "istrator",
                Email = "Admin.CMS@vcn.com",
                Password = "admin",
                OfficeId = adminOffice.Id,
                Office = adminOffice,
                Role = UserRole.Admin
            },
            new()
            {
                FirstName = "Henk",
                LastName = "de Rooi",
                Email = "Henk.Rooi@vcn.com",
                Password = "wachtwoord",
                OfficeId = office.Id,
                Office = office
            }
        };

        foreach (var user in users)
        {
            user.Password = passwordHasher.HashPassword(user, user.Password);
        }

        await Context.Users.AddRangeAsync(users);
        await Context.SaveChangesAsync();

        Logger.LogInformation("Seeded {Count} users", users.Count);
    }
}