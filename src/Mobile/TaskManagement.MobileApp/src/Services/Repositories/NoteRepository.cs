using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task.Note;
using TaskManagement.MobileApp.Services.Authentication.Utils;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class NoteRepository(NoteClient client, AuthenticatedRequestExecutor executor): INoteRepository
{

    public async Task<List<NoteResponse>> GetNotesAsync(Guid taskId)
    {
        return await executor.Execute(async () =>
        {
            var tasks = await client.GetNotesByTaskAsync(taskId);
            return tasks.ToList();
        });
    }

    public async Task<NoteResponse> CreateNoteAsync(CreateNote task)
    {
        return await executor.Execute(() => client.CreateNoteAsync(task));
    }

    public async Task DeleteNoteAsync(Guid noteId)
    {
        await executor.Execute(() => client.DeleteNoteAsync(noteId));
    }
}