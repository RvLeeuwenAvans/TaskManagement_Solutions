using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class UserRepository(IDbContext context) : IUserRepository
{
    public IQueryable<User> GetAll()
    {
        return context.Users.AsQueryable();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is not null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}