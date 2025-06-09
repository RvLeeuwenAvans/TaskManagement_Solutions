using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services.Repositories;

public class UserRepository(UserClient client) : IUserRepository
{
    public async Task<UserResponse> GetUserById(Guid userId)
    {
        return await client.GetUserByIdAsync(userId);
    }

    public async Task<List<UserResponse>> GetUsersByOfficeAsync(Guid officeId)
    {
        var response = await client.GetUsersByOfficeAsync(officeId);
        return response.ToList();
    }
}