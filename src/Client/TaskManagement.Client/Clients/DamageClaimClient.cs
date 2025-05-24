using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.Client.Clients;

public class DamageClaimClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<DamageClaimResponseDto>> GetClaimsByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<DamageClaimResponseDto>>($"/api/DamageClaim/office/{officeId}",
            cancellationToken);
    }

    public async Task<DamageClaimResponseDto> GetClaimByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<DamageClaimResponseDto>($"/api/DamageClaim/{id}", cancellationToken);
    }

    public async Task<DamageClaimResponseDto> CreateClaimAsync(DamageClaimCreateDto claim,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<DamageClaimCreateDto, DamageClaimResponseDto>("/api/DamageClaim", claim,
            cancellationToken);
    }

    public async Task UpdateClaimAsync(Guid id, DamageClaimUpdateDto claim,
        CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/DamageClaim/{id}", claim, cancellationToken);
    }

    public async Task DeleteClaimAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/DamageClaim/{id}", cancellationToken);
    }
}