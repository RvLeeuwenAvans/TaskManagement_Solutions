using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.Client.Clients;

public class DamageClaimClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<DamageClaimResponse>> GetClaimsByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<DamageClaimResponse>>($"/api/DamageClaim/office/{officeId}",
            cancellationToken);
    }

    public async Task<DamageClaimResponse> GetClaimByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<DamageClaimResponse>($"/api/DamageClaim/{id}", cancellationToken);
    }

    public async Task<DamageClaimResponse> CreateClaimAsync(CreateDamageClaim claim,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateDamageClaim, DamageClaimResponse>("/api/DamageClaim", claim,
            cancellationToken);
    }

    public async Task UpdateClaimAsync(Guid id, UpdateDamageClaim claim,
        CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/DamageClaim/{id}", claim, cancellationToken);
    }

    public async Task DeleteClaimAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/DamageClaim/{id}", cancellationToken);
    }
}