using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class RelationRepository(RelationClient client) : IRelationRepository
{
    public async Task<RelationResponse> GetRelationAsync(Guid relationId)
    {
        return await client.GetRelationByIdAsync(relationId);
    }
}