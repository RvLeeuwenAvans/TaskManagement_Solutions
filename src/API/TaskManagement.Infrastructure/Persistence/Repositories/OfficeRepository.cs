using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class OfficeRepository(IDbContext context) : IOfficeRepository
{
    public IQueryable<Office> GetAll()
    {
        return context.Offices.AsQueryable();
    }

    public async Task<Office?> GetByIdAsync(Guid id)
    {
        return await context.Offices.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Office office)
    {
        await context.Offices.AddAsync(office);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Office office)
    {
        context.Offices.Update(office);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var office = await context.Offices.FindAsync(id);
        if (office is not null)
        {
            context.Offices.Remove(office);
            await context.SaveChangesAsync();
        }
    }
}