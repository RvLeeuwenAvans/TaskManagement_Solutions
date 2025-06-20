using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Interfaces;
using TaskManagement.DTO.Office;

namespace TaskManagement.CMS.Services;

public class OfficeService : IOfficeService
{
    private readonly OfficeClient _officeClient;
    private readonly ILogger<OfficeService> _logger;

    public OfficeService(OfficeClient officeClient, ILogger<OfficeService> logger)
    {
        _officeClient = officeClient;
        _logger = logger;
    }

    public async Task<IEnumerable<OfficeResponse>> GetAllOfficesAsync()
    {
        try
        {
            return await _officeClient.GetAllOfficesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all offices");
            throw;
        }
    }

    public async Task<OfficeResponse> GetOfficeByIdAsync(Guid id)
    {
        try
        {
            return await _officeClient.GetOfficeByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting office with id {OfficeId}", id);
            throw;
        }
    }

    public async Task<OfficeResponse> CreateOfficeAsync(CreateOffice office)
    {
        try
        {
            return await _officeClient.CreateOfficeAsync(office);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating office {OfficeName}", office.Name);
            throw;
        }
    }

    public async Task UpdateOfficeAsync(Guid id, UpdateOffice office)
    {
        try
        {
            await _officeClient.UpdateOfficeAsync(id, office);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating office with id {OfficeId}", id);
            throw;
        }
    }

    public async Task DeleteOfficeAsync(Guid id)
    {
        try
        {
            await _officeClient.DeleteOfficeAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting office with id {OfficeId}", id);
            throw;
        }
    }
}