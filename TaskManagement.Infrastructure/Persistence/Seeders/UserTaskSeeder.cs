using TaskManagement.Domain.Office.User.Task;

namespace TaskManagement.Infrastructure.Persistence.Seeders;

public static class UserTaskSeeder
{
    public static async Task SeedAsync(TaskManagementDatabaseContext context)
    {
        if (!context.Tasks.Any())
        {
            var user = context.Users.First();
            var tasks = new List<UserTask>
            {
                new() { Title = "Task 1", Description = "Description 1", UserId = user.Id, User = user, CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(1) },
                new() { Title = "Task 2", Description = "Description 2", UserId = user.Id, User = user, CreatorName = user.FirstName, DueDate = DateTime.Now.AddDays(1) }
            };

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();
        }
    }
}