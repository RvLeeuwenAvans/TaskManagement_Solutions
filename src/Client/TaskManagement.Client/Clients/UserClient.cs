using TaskManagement.DTO.Office.User;

namespace TaskManagement.Client.Clients;

public class UserClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<ResponseUser>> GetUsersByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<ResponseUser>>($"/api/User/office/{officeId}", cancellationToken);
    }

    public async Task<ResponseUser> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<ResponseUser>($"/api/User/{id}", cancellationToken);
    }

    public async Task<ResponseUser> CreateUserAsync(CreateUser user,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateUser, ResponseUser>("/api/User", user, cancellationToken);
    }

    public async Task UpdateUserAsync(Guid id, UpdateUser user, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/User/{id}", user, cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/User/{id}", cancellationToken);
    }
}