using TaskManagement.Domain.Office.User.Task.LinkedObject;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface ILinkedObjectRepository
{
    IQueryable<LinkedObject> GetAll();
    Task<LinkedObject?> GetByIdAsync(Guid id);
    Task AddAsync(LinkedObject linkedObject);
    Task UpdateAsync(LinkedObject linkedObject);
    Task DeleteAsync(Guid id);
}