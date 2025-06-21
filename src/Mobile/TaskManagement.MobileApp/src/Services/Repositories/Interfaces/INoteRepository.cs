using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface INoteRepository
{
    Task<List<NoteResponse>> GetNotesAsync(Guid taskId);
    
    Task<NoteResponse> CreateNoteAsync(CreateNote task);
    
    Task DeleteNoteAsync(Guid noteId);
}