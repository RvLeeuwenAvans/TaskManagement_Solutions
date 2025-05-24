using TaskManagement.Domain.Office.User.Task.Note;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface INoteRepository
{
    IQueryable<Note> GetAll();
    Task<Note?> GetByIdAsync(Guid id);
    Task AddAsync(Note note);
    Task UpdateAsync(Note note);
    Task DeleteAsync(Guid id);
}