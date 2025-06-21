using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Domain.Office.User.Task;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public class UserTaskSeeder(TaskManagementDatabaseContext context, ILogger<UserTaskSeeder> logger)
    : BaseSeeder(context, logger)
{
    public override int Order => 6;
    public override string Name => "User Tasks";

    public override async Task SeedAsync()
    {
        if (await HasDataAsync<UserTask>())
        {
            Logger.LogInformation("User tasks already seeded, skipping...");
            return;
        }

        Logger.LogInformation("Seeding user tasks...");

        var user = await Context.Users.FirstAsync(u => u.Email == "Marianne.Rodepanne@vcn.com");
        var tasks = new List<UserTask>
        {
            new()
            {
                Title = "Onderhoud doorvoeren", Description = "Klant is al eeen tijd niet gecontacteerd.",
                UserId = user.Id, User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(0)
            },
            new()
            {
                Title = "Controle gesprekken inplannen",
                Description = "Loop het relatie bestand door en kijk of er controle gesprekken nodig zijn.",
                UserId = user.Id, User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(-2)
            },
            new()
            {
                Title = "Documenten doornemen",
                Description = "Documenten toegeovegd op schadeclaim moeten door worden genomen", UserId = user.Id,
                User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(2)
            },
        };

        await Context.Tasks.AddRangeAsync(tasks);
        await Context.SaveChangesAsync();

        Logger.LogInformation("Seeded {Count} user tasks", tasks.Count);
    }
}