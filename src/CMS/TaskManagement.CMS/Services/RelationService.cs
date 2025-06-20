using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.CMS.Services;

public class RelationService(RelationClient relationClient)
{
    public async Task<List<RelationResponse>> GetByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return (await relationClient.GetRelationsByOfficeAsync(officeId, cancellationToken)).ToList();
    }

    public async Task<RelationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await relationClient.GetRelationByIdAsync(id, cancellationToken);
    }

    public async Task<RelationResponse> CreateAsync(CreateRelation relation,
        CancellationToken cancellationToken = default)
    {
        return await relationClient.CreateRelationAsync(relation, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, UpdateRelation relation, CancellationToken cancellationToken = default)
    {
        await relationClient.UpdateRelationAsync(id, relation, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await relationClient.DeleteRelationAsync(id, cancellationToken);
    }
}