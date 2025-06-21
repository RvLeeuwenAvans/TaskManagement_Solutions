using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task.LinkedObject;
using TaskManagement.MobileApp.Services.Authentication.Utils;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class LinkedObjectRepository(LinkedObjectClient client, AuthenticatedRequestExecutor executor) : ILinkedObjectRepository
{
    public async Task<LinkedObjectResponse> CreateLinkedObjectAsync(CreateLinkedObject dto)
    {
        return await executor.Execute(() => client.CreateLinkedObjectAsync(dto));
    }
}