using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office;
using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class OfficeRepository(OfficeClient client) : IOfficeRepository
{
    public async Task<OfficeResponse> GetOfficeById(Guid officeId)
    {
        return await client.GetOfficeByIdAsync(officeId);
    }
}