using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office;

namespace TaskManagement.CMS.Services;

public class OfficeService(OfficeClient officeClient, UserAuthenticationClient authenticationClient)
{
    public async Task<List<OfficeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await authenticationClient.AuthenticateUserAsync("jane.smith@example.com", "hashedpassword23");
        return (await officeClient.GetAllOfficesAsync(cancellationToken)).ToList();
    }

    public async Task<OfficeResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await officeClient.GetOfficeByIdAsync(id, cancellationToken);
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var dto = new CreateOffice { Name = name };
        await officeClient.CreateOfficeAsync(dto, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, string name, CancellationToken cancellationToken = default)
    {
        var dto = new UpdateOffice { Id = id, Name = name };
        await officeClient.UpdateOfficeAsync(id, dto, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await officeClient.DeleteOfficeAsync(id, cancellationToken);
    }
}
