using TaskManagement.DTO.Office;

namespace TaskManagement.Client.Clients;

public class OfficeClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<OfficeResponseDto>> GetAllOfficesAsync(CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<OfficeResponseDto>>("/api/Office", cancellationToken);
    }

    public async Task<OfficeResponseDto> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<OfficeResponseDto>($"/api/Office/{id}", cancellationToken);
    }

    public async Task<OfficeResponseDto> CreateOfficeAsync(OfficeCreateDto office,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<OfficeCreateDto, OfficeResponseDto>("/api/Office", office, cancellationToken);
    }

    public async Task UpdateOfficeAsync(Guid id, OfficeUpdateDto office, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/Office/{id}", office, cancellationToken);
    }

    public async Task DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/Office/{id}", cancellationToken);
    }
}