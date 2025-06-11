using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class UserRepository(UserClient client, AuthenticatedEndpointExecutor executor) : IUserRepository
{
    public async Task<UserResponse> GetUserById(Guid userId)
    {
        return await executor.Execute(() => client.GetUserByIdAsync(userId));
    }

    public async Task<List<UserResponse>> GetUsersByOfficeAsync(Guid officeId)
    {
        return await executor.Execute(async () =>
        {
            var response = await client.GetUsersByOfficeAsync(officeId);
            return response.ToList();
        });
    }
}