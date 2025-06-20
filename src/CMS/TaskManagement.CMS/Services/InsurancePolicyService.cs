using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.CMS.Services;

public class InsurancePolicyService(InsurancePolicyClient insuranceClient)
{
    public async Task<List<InsurancePolicyResponse>> GetByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return (await insuranceClient.GetPoliciesByOfficeAsync(officeId, cancellationToken)).ToList();
    }

    public async Task<InsurancePolicyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await insuranceClient.GetPolicyByIdAsync(id, cancellationToken);
    }

    public async Task<InsurancePolicyResponse> CreateAsync(CreateInsurancePolicy policy,
        CancellationToken cancellationToken = default)
    {
        return await insuranceClient.CreatePolicyAsync(policy, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, UpdateInsurancePolicy policy, CancellationToken cancellationToken = default)
    {
        await insuranceClient.UpdatePolicyAsync(id, policy, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await insuranceClient.DeletePolicyAsync(id, cancellationToken);
    }
}