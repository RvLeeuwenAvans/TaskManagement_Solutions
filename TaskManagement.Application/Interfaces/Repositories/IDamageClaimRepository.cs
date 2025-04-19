using TaskManagement.Domain.Office.Relation.DamageClaim;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IDamageClaimRepository
{
    IQueryable<DamageClaim> GetAll();
    Task<DamageClaim?> GetByIdAsync(Guid id);
    Task AddAsync(DamageClaim damageClaim);
    Task UpdateAsync(DamageClaim damageClaim);
    Task DeleteAsync(Guid id);
}