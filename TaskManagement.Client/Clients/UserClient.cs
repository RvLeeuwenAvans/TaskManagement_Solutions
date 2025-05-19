using TaskManagement.DTO.Office.User;

namespace TaskManagement.Client.Clients;

public class UserClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<IEnumerable<UserResponseDto>> GetUsersByOfficeAsync(Guid officeId,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<IEnumerable<UserResponseDto>>($"/api/User/office/{officeId}", cancellationToken);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync<UserResponseDto>($"/api/User/{id}", cancellationToken);
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto user,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<UserCreateDto, UserResponseDto>("/api/User", user, cancellationToken);
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateDto user, CancellationToken cancellationToken = default)
    {
        await PutAsync($"/api/User/{id}", user, cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteAsync($"/api/User/{id}", cancellationToken);
    }
}