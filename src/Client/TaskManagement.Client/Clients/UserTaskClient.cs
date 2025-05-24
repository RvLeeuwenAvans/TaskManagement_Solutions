using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.Client.Clients;

public class UserTaskClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<UserTaskResponseDto>> GetTasksByUserAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<UserTaskResponseDto>>($"/api/UserTask/user/{userId}", cancellationToken);
    }

    public async Task<UserTaskResponseDto> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<UserTaskResponseDto>($"/api/UserTask/{id}", cancellationToken);
    }

    public async Task<UserTaskResponseDto> CreateTaskAsync(UserTaskCreateDto task,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<UserTaskCreateDto, UserTaskResponseDto>("/api/UserTask", task, cancellationToken);
    }

    public async Task UpdateTaskAsync(Guid id, UserTaskUpdateDto task, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/UserTask/{id}", task, cancellationToken);
    }

    public async Task DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/UserTask/{id}", cancellationToken);
    }
}