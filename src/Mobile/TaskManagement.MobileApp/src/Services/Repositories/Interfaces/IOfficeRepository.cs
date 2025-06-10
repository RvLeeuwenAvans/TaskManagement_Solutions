using TaskManagement.DTO.Office;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IOfficeRepository
{
    Task<OfficeResponse> GetOfficeById(Guid officeId);
}