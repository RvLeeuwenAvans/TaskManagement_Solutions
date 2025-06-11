using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class InsurancePolicyRepository(InsurancePolicyClient client, AuthenticatedEndpointExecutor executor) : IPolicyRepository
{
    public async Task<InsurancePolicyResponse> GetInsurancePolicyAsync(Guid policyId)
    {
        return await executor.Execute(() => client.GetPolicyByIdAsync(policyId));
    }

    public async Task<List<InsurancePolicyResponse>> GetInsurancePoliciesByOfficeAsync(Guid officeId)
    {
        return await executor.Execute(async () =>
        {
            var response = await client.GetPoliciesByOfficeAsync(officeId);
            return response.ToList();
        });
    }
}