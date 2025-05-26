using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class DamageClaimRepository(DamageClaimClient client) : IDamageClaimRepository
{
    public async Task<DamageClaimResponse> GetDamageClaimAsync(Guid damageClaimId)
    {
        return await client.GetClaimByIdAsync(damageClaimId);
    }
}