using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services;

public class UserService(UserClient userClient)
{
    public async Task<List<UserResponse>> GetByOfficeAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        return (await userClient.GetUsersByOfficeAsync(officeId, cancellationToken)).ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await userClient.GetUserByIdAsync(id, cancellationToken);
    }

    public async Task<UserResponse> CreateAsync(CreateUser user, CancellationToken cancellationToken = default)
    {
        return await userClient.CreateUserAsync(user, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, UpdateUser user, CancellationToken cancellationToken = default)
    {
        await userClient.UpdateUserAsync(id, user, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await userClient.DeleteUserAsync(id, cancellationToken);
    }
}