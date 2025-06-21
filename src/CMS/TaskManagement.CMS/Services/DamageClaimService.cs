using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.CMS.Services;

public class DamageClaimService(DamageClaimClient client, AuthenticationService authenticationService)
    : AuthenticatedServiceBase(authenticationService)
{
    public async Task<List<DamageClaimResponse>> GetByRelationAsync(Guid relationId,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetClaimsByRelationAsync(relationId, cancellationToken)
                .ContinueWith(t => t.Result.ToList(), cancellationToken));

    public async Task<DamageClaimResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetClaimByIdAsync(id, cancellationToken));

    public async Task CreateAsync(Guid relationId, string type, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var createDto = new CreateDamageClaim
            {
                RelationId = relationId,
                Type = type
            };
            return client.CreateClaimAsync(createDto, cancellationToken);
        });

    public async Task UpdateAsync(Guid id, string type, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var updateDto = new UpdateDamageClaim
            {
                Id = id,
                Type = type
            };
            return client.UpdateClaimAsync(id, updateDto, cancellationToken);
        });

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.DeleteClaimAsync(id, cancellationToken));
}