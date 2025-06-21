using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.CMS.Services;

public class RelationService(RelationClient client, AuthenticationService authenticationService)
    : AuthenticatedServiceBase(authenticationService)
{
    public async Task<List<RelationResponse>> GetByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetRelationsByOfficeAsync(officeId, cancellationToken)
                .ContinueWith(t => t.Result?.ToList() ?? new List<RelationResponse>(), cancellationToken));

    public async Task<RelationResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.GetRelationByIdAsync(id, cancellationToken));

    public async Task CreateAsync(Guid officeId, string firstName, string lastName,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var createDto = new CreateRelation
            {
                OfficeId = officeId,
                FirstName = firstName,
                LastName = lastName
            };
            return client.CreateRelationAsync(createDto, cancellationToken);
        });

    public async Task UpdateAsync(Guid id, string firstName, string lastName,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var updateDto = new UpdateRelation
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };
            return client.UpdateRelationAsync(id, updateDto, cancellationToken);
        });

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            client.DeleteRelationAsync(id, cancellationToken));
}