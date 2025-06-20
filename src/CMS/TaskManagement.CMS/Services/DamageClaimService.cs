using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.CMS.Services;

public class DamageClaimService(DamageClaimClient client)
{
    public async Task<List<DamageClaimResponse>> GetByOfficeAsync(Guid officeId)
    {
        var claims = await client.GetClaimsByOfficeAsync(officeId);
        return claims.ToList();
    }

    public async Task<DamageClaimResponse?> GetByIdAsync(Guid id)
    {
        return await client.GetClaimByIdAsync(id);
    }

    public async Task<DamageClaimResponse> CreateAsync(Guid relationId, string type)
    {
        var createDto = new CreateDamageClaim
        {
            RelationId = relationId,
            Type = type
        };

        return await client.CreateClaimAsync(createDto);
    }

    public async Task UpdateAsync(Guid id, string type)
    {
        var updateDto = new UpdateDamageClaim
        {
            Id = id,
            Type = type
        };

        await client.UpdateClaimAsync(id, updateDto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await client.DeleteClaimAsync(id);
    }
}