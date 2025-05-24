using TaskManagement.DTO.Office.User.Task.Note;

namespace TaskManagement.Client.Clients;

public class NoteClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<NoteResponseDto>> GetNotesByTaskAsync(Guid taskId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<NoteResponseDto>>($"/api/Note/task/{taskId}", cancellationToken);
    }

    public async Task<NoteResponseDto> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<NoteResponseDto>($"/api/Note/{id}", cancellationToken);
    }

    public async Task<NoteResponseDto> CreateNoteAsync(NoteCreateDto note,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<NoteCreateDto, NoteResponseDto>("/api/Note", note, cancellationToken);
    }

    public async Task UpdateNoteAsync(Guid id, NoteUpdateDto note, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Note/{id}", note, cancellationToken);
    }

    public async Task DeleteNoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Note/{id}", cancellationToken);
    }
}