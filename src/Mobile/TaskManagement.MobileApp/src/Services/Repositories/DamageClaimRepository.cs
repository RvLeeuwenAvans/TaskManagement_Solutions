using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class DamageClaimRepository(DamageClaimClient client, AuthenticatedEndpointExecutor executor) : IDamageClaimRepository
{
    public async Task<DamageClaimResponse> GetDamageClaimAsync(Guid damageClaimId)
    {
        return await executor.Execute(() => client.GetClaimByIdAsync(damageClaimId));
    }

    public async Task<List<DamageClaimResponse>> GetDamageClaimsByOfficeAsync(Guid officeId)
    {
        return await executor.Execute(async () =>
        {
            var response = await client.GetClaimsByOfficeAsync(officeId);
            return response.ToList();
        });
    }
}