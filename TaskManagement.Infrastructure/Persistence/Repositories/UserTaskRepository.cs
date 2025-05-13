using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class UserTaskRepository(IDbContext context) : IUserTaskRepository
{
    public IQueryable<UserTask> GetAll()
    {
        return context.Tasks.Include(task => task.User).AsQueryable();
    }

    public async Task<UserTask?> GetByIdAsync(Guid id)
    {
        return await context.Tasks.Include(task => task.User).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(UserTask task)
    {
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserTask task)
    {
        context.Tasks.Update(task);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task is not null)
        {
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
        }
    }
}