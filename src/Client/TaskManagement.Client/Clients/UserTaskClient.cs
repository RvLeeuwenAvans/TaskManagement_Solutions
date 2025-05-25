using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Client.Clients;

public class UserTaskClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<UserTaskResponse>> GetTasksByUserAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<UserTaskResponse>>($"/api/UserTask/user/{userId}", cancellationToken);
    }

    public async Task<UserTaskResponse> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<UserTaskResponse>($"/api/UserTask/{id}", cancellationToken);
    }

    public async Task<UserTaskResponse> CreateTaskAsync(CreateUserTask task,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateUserTask, UserTaskResponse>("/api/UserTask", task, cancellationToken);
    }

    public async Task UpdateTaskAsync(Guid id, UpdateUserTask task, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/UserTask/{id}", task, cancellationToken);
    }

    public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/UserTask/{id}", cancellationToken);
    }
}