using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Client.Clients;

public class NoteClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<NoteResponse>> GetNotesByTaskAsync(Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<NoteResponse>>($"/api/Note/task/{taskId}", cancellationToken);
    }

    public async Task<NoteResponse> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<NoteResponse>($"/api/Note/{id}", cancellationToken);
    }

    public async Task<NoteResponse> CreateNoteAsync(CreateNote note,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateNote, NoteResponse>("/api/Note", note, cancellationToken);
    }

    public async Task UpdateNoteAsync(Guid id, UpdateNote note, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Note/{id}", note, cancellationToken);
    }

    public async Task DeleteNoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Note/{id}", cancellationToken);
    }
}