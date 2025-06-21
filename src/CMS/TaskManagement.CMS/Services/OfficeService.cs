using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.DTO.Office;

namespace TaskManagement.CMS.Services;

public class OfficeService(OfficeClient officeClient, AuthenticationService authenticationService)
{
    public async Task<List<OfficeResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            officeClient.GetAllOfficesAsync(cancellationToken)
                .ContinueWith(t => t.Result.ToList(), cancellationToken));

    public async Task<OfficeResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            officeClient.GetOfficeByIdAsync(id, cancellationToken));

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            officeClient.CreateOfficeAsync(new CreateOffice { Name = name }, cancellationToken));

    public async Task UpdateAsync(Guid id, string name, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            officeClient.UpdateOfficeAsync(id, new UpdateOffice { Id = id, Name = name }, cancellationToken));

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            officeClient.DeleteOfficeAsync(id, cancellationToken));

    private async Task<T> ExecuteIfAuthenticatedAsync<T>(Func<Task<T>> action)
    {
        if (!await authenticationService.EnsureAuthenticatedAsync())
            throw new UnauthorizedAccessException("User must be authenticated to access this resource.");

        return await authenticationService.ExecuteAuthenticatedRequest(action);
    }

    private async Task ExecuteIfAuthenticatedAsync(Func<Task> action)
    {
        if (!await authenticationService.EnsureAuthenticatedAsync())
            throw new UnauthorizedAccessException("User must be authenticated to access this resource.");

        await authenticationService.ExecuteAuthenticatedRequest(action);
    }
}