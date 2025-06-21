using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;

namespace TaskManagement.CMS.Services;

public class InsurancePolicyService(InsurancePolicyClient client, AuthenticationService authenticationService)
    : AuthenticatedServiceBase(authenticationService)
{
    public async Task<List<InsurancePolicyResponse>> GetByRelationAsync(Guid relationId,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetPoliciesByRelationAsync(relationId, cancellationToken)
                .ContinueWith(t => t.Result.ToList(), cancellationToken));

    public async Task<InsurancePolicyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetPolicyByIdAsync(id, cancellationToken));

    public async Task CreateAsync(Guid relationId, string type, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var createDto = new CreateInsurancePolicy
            {
                RelationId = relationId,
                Type = type
            };
            return client.CreatePolicyAsync(createDto, cancellationToken);
        });

    public async Task UpdateAsync(Guid id, string? type, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var updateDto = new UpdateInsurancePolicy
            {
                Id = id,
                Type = type
            };
            return client.UpdatePolicyAsync(id, updateDto, cancellationToken);
        });

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.DeletePolicyAsync(id, cancellationToken));
}