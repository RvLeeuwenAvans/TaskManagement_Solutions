using TaskManagement.DTO.Office;

namespace TaskManagement.CMS.Services.Interfaces;

public interface IOfficeService
{
    Task<IEnumerable<OfficeResponse>> GetAllOfficesAsync();
    Task<OfficeResponse> GetOfficeByIdAsync(Guid id);
    Task<OfficeResponse> CreateOfficeAsync(CreateOffice office);
    Task UpdateOfficeAsync(Guid id, UpdateOffice office);
    Task DeleteOfficeAsync(Guid id);
}