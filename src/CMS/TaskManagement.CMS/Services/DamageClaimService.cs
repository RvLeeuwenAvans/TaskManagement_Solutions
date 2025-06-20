using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.CMS.Services;

public class DamageClaimService(DamageClaimClient damageClaimClient)
{
    public async Task<List<DamageClaimResponse>> GetByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return (await damageClaimClient.GetClaimsByOfficeAsync(officeId, cancellationToken)).ToList();
    }

    public async Task<DamageClaimResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await damageClaimClient.GetClaimByIdAsync(id, cancellationToken);
    }

    public async Task<DamageClaimResponse> CreateAsync(CreateDamageClaim claim,
        CancellationToken cancellationToken = default)
    {
        return await damageClaimClient.CreateClaimAsync(claim, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, UpdateDamageClaim claim, CancellationToken cancellationToken = default)
    {
        await damageClaimClient.UpdateClaimAsync(id, claim, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await damageClaimClient.DeleteClaimAsync(id, cancellationToken);
    }
}