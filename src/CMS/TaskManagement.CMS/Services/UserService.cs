using TaskManagement.Client.Clients;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services;

public class UserService(UserClient userClient)
{
    public async Task<List<UserResponse>> GetByOfficeAsync(Guid officeId, CancellationToken cancellationToken = default)
    {
        var users = await userClient.GetUsersByOfficeAsync(officeId, cancellationToken);
        return users.ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await userClient.GetUserByIdAsync(id, cancellationToken);
    }

    public async Task CreateAsync(Guid officeId,
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var dto = new CreateUser
        {
            OfficeId = officeId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        await userClient.CreateUserAsync(dto, cancellationToken);
    }

    public async Task UpdateAsync(
        Guid id,
        string? firstName,
        string? lastName,
        string? email,
        string? password,
        CancellationToken cancellationToken = default)
    {
        var dto = new UpdateUser
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        await userClient.UpdateUserAsync(id, dto, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await userClient.DeleteUserAsync(id, cancellationToken);
    }
}