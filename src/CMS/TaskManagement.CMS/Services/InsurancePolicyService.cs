using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.CMS.Services;

public class InsurancePolicyService(InsurancePolicyClient client)
{
    public async Task<List<InsurancePolicyResponse>> GetByRelationAsync(Guid relationId)
    {
        var policies = await client.GetPoliciesByRelationAsync(relationId);
        return policies.ToList();
    }

    public async Task<InsurancePolicyResponse?> GetByIdAsync(Guid id)
    {
        return await client.GetPolicyByIdAsync(id);
    }

    public async Task CreateAsync(Guid relationId, string type)
    {
        var createDto = new CreateInsurancePolicy
        {
            RelationId = relationId,
            Type = type
        };

        await client.CreatePolicyAsync(createDto);
    }

    public async Task UpdateAsync(Guid id, string? type)
    {
        var updateDto = new UpdateInsurancePolicy
        {
            Id = id,
            Type = type
        };

        await client.UpdatePolicyAsync(id, updateDto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await client.DeletePolicyAsync(id);
    }
}