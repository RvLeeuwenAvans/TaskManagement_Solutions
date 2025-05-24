using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation.DamageClaim;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class DamageClaimRepository(IDbContext context) : IDamageClaimRepository
{
    public IQueryable<DamageClaim> GetAll()
    {
        return context.DamageClaims.AsQueryable();
    }

    public async Task<DamageClaim?> GetByIdAsync(Guid id)
    {
        return await context.DamageClaims.FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(DamageClaim damageClaim)
    {
        await context.DamageClaims.AddAsync(damageClaim);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DamageClaim damageClaim)
    {
        context.DamageClaims.Update(damageClaim);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var damageClaim = await context.DamageClaims.FindAsync(id);
        if (damageClaim != null)
        {
            context.DamageClaims.Remove(damageClaim);
            await context.SaveChangesAsync();
        }
    }
}