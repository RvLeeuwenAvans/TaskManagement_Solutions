using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User.Task.LinkedObject;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class LinkedObjectRepository(LinkedObjectClient client) : ILinkedObjectRepository
{
    public async Task<LinkedObjectResponse> CreateLinkedObjectAsync(CreateLinkedObject dto)
    {
        return await client.CreateLinkedObjectAsync(dto);
    }
}