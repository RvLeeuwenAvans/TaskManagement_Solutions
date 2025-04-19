using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class RelationRepository(IDbContext context) : IRelationRepository
{
    public IQueryable<Relation> GetAll()
    {
        return context.Relations.AsQueryable();
    }

    public async Task<Relation?> GetByIdAsync(Guid id)
    {
        return await context.Relations.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Relation relation)
    {
        await context.Relations.AddAsync(relation);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Relation relation)
    {
        context.Relations.Update(relation);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var relation = await context.Relations.FindAsync(id);
        if (relation != null)
        {
            context.Relations.Remove(relation);
            await context.SaveChangesAsync();
        }
    }
}