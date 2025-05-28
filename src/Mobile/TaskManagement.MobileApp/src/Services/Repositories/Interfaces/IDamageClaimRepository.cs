using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IDamageClaimRepository
{
    Task<DamageClaimResponse> GetDamageClaimAsync(Guid damageClaimId);
}