using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.Client.Clients;

public class RelationClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<RelationResponse>> GetRelationsByOfficeAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<RelationResponse>>($"/api/Relation/office/{officeId}", cancellationToken);
    }

    public async Task<RelationResponse> GetRelationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<RelationResponse>($"/api/Relation/{id}", cancellationToken);
    }

    public async Task<RelationResponse> CreateRelationAsync(CreateRelation relation, CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateRelation, RelationResponse>("/api/Relation", relation, cancellationToken);
    }

    public async Task UpdateRelationAsync(Guid id, UpdateRelation relation, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Relation/{id}", relation, cancellationToken);
    }

    public async Task DeleteRelationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Relation/{id}", cancellationToken);
    }
}
