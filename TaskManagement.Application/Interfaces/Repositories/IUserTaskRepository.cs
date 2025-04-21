using TaskManagement.Domain.Office.User.Task;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IUserTaskRepository
{
    IQueryable<UserTask> GetAll();
    Task<UserTask?> GetByIdAsync(Guid id);
    Task AddAsync(UserTask task);
    Task UpdateAsync(UserTask task);
    Task DeleteAsync(Guid id);
}