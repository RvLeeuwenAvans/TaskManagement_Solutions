using TaskManagement.Domain.Office;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IOfficeRepository
{
    IQueryable<Office> GetAll();
    Task<Office?> GetByIdAsync(Guid id);
    Task AddAsync(Office office);
    Task UpdateAsync(Office office);
    Task DeleteAsync(Guid id);
}