using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Authentication;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services;

public class UserService(UserClient userClient, AuthenticationService authenticationService)
    : AuthenticatedServiceBase(authenticationService)
{
    public async Task<List<UserResponse>>
        GetByOfficeAsync(Guid officeId, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            userClient.GetUsersByOfficeAsync(officeId, cancellationToken)
                .ContinueWith(t => t.Result.ToList(), cancellationToken));

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            userClient.GetUserByIdAsync(id, cancellationToken));

    public async Task CreateAsync(Guid officeId, string firstName, string lastName, string email, string password,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var dto = new CreateUser
            {
                OfficeId = officeId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            return userClient.CreateUserAsync(dto, cancellationToken);
        });

    public async Task UpdateAsync(Guid id, string? firstName, string? lastName, string? email, string? password,
        CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
        {
            var dto = new UpdateUser
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            return userClient.UpdateUserAsync(id, dto, cancellationToken);
        });

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteIfAuthenticatedAsync(() =>
            userClient.DeleteUserAsync(id, cancellationToken));
}