using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task.LinkedObject;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class LinkedObjectRepository(IDbContext context) : ILinkedObjectRepository
{
    public IQueryable<LinkedObject> GetAll()
    {
        return context.LinkedObjects.AsQueryable();
    }

    public async Task<LinkedObject?> GetByIdAsync(Guid id)
    {
        return await context.LinkedObjects.FirstOrDefaultAsync(lo => lo.Id == id);
    }

    public async Task AddAsync(LinkedObject linkedObject)
    {
        await context.LinkedObjects.AddAsync(linkedObject);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(LinkedObject linkedObject)
    {
        context.LinkedObjects.Update(linkedObject);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.LinkedObjects.FindAsync(id);
        if (entity != null)
        {
            context.LinkedObjects.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}