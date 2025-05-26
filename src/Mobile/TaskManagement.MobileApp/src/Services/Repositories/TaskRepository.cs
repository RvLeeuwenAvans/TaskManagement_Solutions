using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class TaskRepository(UserTaskClient client) : ITaskRepository
{
    public async Task<List<UserTaskResponse>> GetTasksAsync(Guid userId)
    {
        var tasks = await client.GetTasksByUserAsync(userId);
        return tasks.ToList();
    }
}