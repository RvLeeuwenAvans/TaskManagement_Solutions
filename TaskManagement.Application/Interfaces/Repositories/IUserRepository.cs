using TaskManagement.Domain.Office.User;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IUserRepository
{
    IQueryable<User> GetAll();
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
}