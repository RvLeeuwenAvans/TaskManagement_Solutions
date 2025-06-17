using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class OfficeRepository(OfficeClient client, AuthenticatedRequestExecutor executor) : IOfficeRepository
{
    public async Task<OfficeResponse> GetOfficeById(Guid officeId)
    {
        return await executor.Execute(() => client.GetOfficeByIdAsync(officeId));
    }
}