using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Client.Clients;

public class InsurancePolicyClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<InsurancePolicyResponseDto>> GetPoliciesByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<InsurancePolicyResponseDto>>($"/api/InsurancePolicy/office/{officeId}",
            cancellationToken);
    }

    public async Task<InsurancePolicyResponseDto> GetPolicyByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<InsurancePolicyResponseDto>($"/api/InsurancePolicy/{id}", cancellationToken);
    }

    public async Task<InsurancePolicyResponseDto> CreatePolicyAsync(InsurancePolicyCreateDto policy,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<InsurancePolicyCreateDto, InsurancePolicyResponseDto>("/api/InsurancePolicy", policy,
            cancellationToken);
    }

    public async Task UpdatePolicyAsync(Guid id, InsurancePolicyUpdateDto policy,
        CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/InsurancePolicy/{id}", policy, cancellationToken);
    }

    public async Task DeletePolicyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/InsurancePolicy/{id}", cancellationToken);
    }
}