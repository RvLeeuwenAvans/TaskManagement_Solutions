using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Repositories;

public class TaskRepository(UserTaskClient client) : ITaskRepository
{
    public async Task<List<UserTaskResponseDto>> GetTasksAsync(Guid userId)
    {
        var tasks = await client.GetTasksByUserAsync(userId);
        return tasks.ToList();
    }
}