using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.CMS.Services;

public class RelationService
{
    private readonly RelationClient _client;

    public RelationService(RelationClient client)
    {
        _client = client;
    }

    public async Task<List<RelationResponse>> GetByOfficeAsync(Guid officeId)
    {
        var relations = await _client.GetRelationsByOfficeAsync(officeId);
        return relations?.ToList() ?? new List<RelationResponse>();
    }

    public async Task<RelationResponse?> GetByIdAsync(Guid id)
    {
        return await _client.GetRelationByIdAsync(id);
    }

    public async Task CreateAsync(Guid officeId, string firstName, string lastName)
    {
        var createDto = new CreateRelation
        {
            OfficeId = officeId,
            FirstName = firstName,
            LastName = lastName
        };

        await _client.CreateRelationAsync(createDto);
    }

    public async Task UpdateAsync(Guid id, string firstName, string lastName)
    {
        var updateDto = new UpdateRelation
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName
        };

        await _client.UpdateRelationAsync(id, updateDto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _client.DeleteRelationAsync(id);
    }
}