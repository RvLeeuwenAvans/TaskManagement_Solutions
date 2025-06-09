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

    public async Task<UserTaskResponse> GetTaskAsync(Guid taskId)
    {
        return await client.GetTaskByIdAsync(taskId);
    }

    public async Task<UserTaskResponse> CreateTaskAsync(CreateUserTask task)
    {
        return await client.CreateTaskAsync(task);
    }

    public async Task UpdateTaskAsync(UpdateUserTask task)
    {
        await client.UpdateTaskAsync(task.Id, task);
    }

    public async Task CloseTasksAsync(Guid taskId)
    {
        await client.DeleteTaskAsync(taskId);
    }
}