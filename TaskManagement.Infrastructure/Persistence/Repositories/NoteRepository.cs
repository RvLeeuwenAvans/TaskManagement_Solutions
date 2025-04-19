using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.User.Task.Note;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class NoteRepository(IDbContext context) : INoteRepository
{
    public IQueryable<Note> GetAll()
    {
        return context.Notes.AsQueryable();
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        return await context.Notes.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task AddAsync(Note note)
    {
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var note = await context.Notes.FindAsync(id);
        if (note is not null)
        {
            context.Notes.Remove(note);
            await context.SaveChangesAsync();
        }
    }
}