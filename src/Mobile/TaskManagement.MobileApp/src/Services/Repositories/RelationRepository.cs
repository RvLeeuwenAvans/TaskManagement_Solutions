using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class RelationRepository(RelationClient client, AuthenticatedRequestExecutor executor) : IRelationRepository
{
    public async Task<RelationResponse> GetRelationAsync(Guid relationId)
    {
        return await executor.Execute(() => client.GetRelationByIdAsync(relationId));
    }

    public async Task<List<RelationResponse>> GetRelationsByOfficeAsync(Guid officeId)
    {
        return await executor.Execute(async () =>
        {
            var response = await client.GetRelationsByOfficeAsync(officeId);
            return response.ToList();
        });
    }
}