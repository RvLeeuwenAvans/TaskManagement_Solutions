using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTaskResponse>> GetTasksAsync(Guid userId);
    
    Task<UserTaskResponse> GetTaskAsync(Guid taskId);
    
    Task<UserTaskResponse> CreateTaskAsync(CreateUserTask task);

    Task UpdateTaskAsync(UpdateUserTask task);
    
    Task CloseTasksAsync(Guid taskId);
}