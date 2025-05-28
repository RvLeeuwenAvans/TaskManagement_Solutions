using TaskManagement.DTO.Office;

namespace TaskManagement.Client.Clients;

public class OfficeClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<OfficeResponse>> GetAllOfficesAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<OfficeResponse>>("/api/Office", cancellationToken);
    }

    public async Task<OfficeResponse> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<OfficeResponse>($"/api/Office/{id}", cancellationToken);
    }

    public async Task<OfficeResponse> CreateOfficeAsync(CreateOffice office,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateOffice, OfficeResponse>("/api/Office", office, cancellationToken);
    }

    public async Task UpdateOfficeAsync(Guid id, UpdateOffice office, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Office/{id}", office, cancellationToken);
    }

    public async Task DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Office/{id}", cancellationToken);
    }
}