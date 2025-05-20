using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Client.Clients;

public class RelationClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<RelationResponseDto>> GetRelationsByOfficeAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<RelationResponseDto>>($"/api/Relation/office/{officeId}", cancellationToken);
    }

    public async Task<RelationResponseDto> GetRelationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<RelationResponseDto>($"/api/Relation/{id}", cancellationToken);
    }

    public async Task<RelationResponseDto> CreateRelationAsync(RelationCreateDto relation, CancellationToken cancellationToken = default)
    {
        return await PostAsync<RelationCreateDto, RelationResponseDto>("/api/Relation", relation, cancellationToken);
    }

    public async Task UpdateRelationAsync(Guid id, RelationUpdateDto relation, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Relation/{id}", relation, cancellationToken);
    }

    public async Task DeleteRelationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Relation/{id}", cancellationToken);
    }
}
