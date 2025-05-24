using TaskManagement.Domain.Office.Relation;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IRelationRepository
{
    IQueryable<Relation> GetAll();
    Task<Relation?> GetByIdAsync(Guid id);
    Task AddAsync(Relation relation);
    Task UpdateAsync(Relation relation);
    Task DeleteAsync(Guid id);
}