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

        var user = await Context.Users.FirstAsync();
        var tasks = new List<UserTask>
        {
            new()
            {
                Title = "Task 1", Description = "Description 1", UserId = user.Id, User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(1)
            },
            new()
            {
                Title = "Task 2", Description = "Description 2", UserId = user.Id, User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(7)
            },
            new()
            {
                Title = "Task 3", Description = "Description 3", UserId = user.Id, User = user,
                CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(2)
            }
        };

        await Context.Tasks.AddRangeAsync(tasks);
        await Context.SaveChangesAsync();

        Logger.LogInformation("Seeded {Count} user tasks", tasks.Count);
    }
}