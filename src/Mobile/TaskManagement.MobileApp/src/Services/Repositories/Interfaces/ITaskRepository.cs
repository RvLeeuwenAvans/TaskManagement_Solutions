using TaskManagement.DTO.Office.User.Task;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTaskResponse>> GetTasksAsync(Guid userId);
}