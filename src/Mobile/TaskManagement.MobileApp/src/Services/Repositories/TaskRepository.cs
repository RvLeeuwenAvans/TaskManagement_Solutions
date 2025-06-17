using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class TaskRepository(UserTaskClient client, AuthenticatedRequestExecutor executor) : ITaskRepository
{
    public async Task<List<UserTaskResponse>> GetTasksAsync(Guid userId)
    {
        return await executor.Execute(async () =>
        {
            var tasks = await client.GetTasksByUserAsync(userId);
            return tasks.ToList();
        });
    }

    public async Task<UserTaskResponse> GetTaskAsync(Guid taskId)
    {
        return await executor.Execute(() => client.GetTaskByIdAsync(taskId));
    }

    public async Task<UserTaskResponse> CreateTaskAsync(CreateUserTask task)
    {
        return await executor.Execute(() => client.CreateTaskAsync(task));
    }

    public async Task UpdateTaskAsync(UpdateUserTask task)
    {
        await executor.Execute(() => client.UpdateTaskAsync(task.Id, task));
    }

    public async Task CloseTasksAsync(Guid taskId)
    {
        await executor.Execute(() => client.DeleteTaskAsync(taskId));
    }
}