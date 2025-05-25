using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<List<UserTaskResponse>> GetTasksAsync(Guid userId);
}