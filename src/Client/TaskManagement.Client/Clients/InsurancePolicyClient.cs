using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.Client.Clients;

public class InsurancePolicyClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<InsurancePolicyResponse>> GetPoliciesByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<InsurancePolicyResponse>>($"/api/InsurancePolicy/office/{officeId}",
            cancellationToken);
    }

    public async Task<IEnumerable<InsurancePolicyResponse>> GetPoliciesByRelationAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<InsurancePolicyResponse>>($"/api/InsurancePolicy/relation/{officeId}",
            cancellationToken);
    }

    public async Task<InsurancePolicyResponse> GetPolicyByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<InsurancePolicyResponse>($"/api/InsurancePolicy/{id}", cancellationToken);
    }

    public async Task<InsurancePolicyResponse> CreatePolicyAsync(CreateInsurancePolicy policy,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateInsurancePolicy, InsurancePolicyResponse>("/api/InsurancePolicy", policy,
            cancellationToken);
    }

    public async Task UpdatePolicyAsync(Guid id, UpdateInsurancePolicy policy,
        CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/InsurancePolicy/{id}", policy, cancellationToken);
    }

    public async Task DeletePolicyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/InsurancePolicy/{id}", cancellationToken);
    }
}