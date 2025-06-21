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

        var office = await Context.Offices.FirstAsync();

        var users = new List<User>
        {
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "hashedpassword12",
                OfficeId = office.Id,
                Office = office
            },
            new()
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Password = "hashedpassword23",
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