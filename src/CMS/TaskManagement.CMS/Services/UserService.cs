using TaskManagement.Client.Clients;
using TaskManagement.CMS.Services.Interfaces;
using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services;

public class UserService : IUserService
{
    private readonly UserClient _userClient;
    private readonly ILogger<UserService> _logger;

    public UserService(UserClient userClient, ILogger<UserService> logger)
    {
        _userClient = userClient;
        _logger = logger;
    }

    public async Task<IEnumerable<UserResponse>> GetUsersByOfficeAsync(Guid officeId)
    {
        try
        {
            return await _userClient.GetUsersByOfficeAsync(officeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users for office {OfficeId}", officeId);
            throw;
        }
    }

    public async Task<UserResponse> GetUserByIdAsync(Guid id)
    {
        try
        {
            return await _userClient.GetUserByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with id {UserId}", id);
            throw;
        }
    }

    public async Task<UserResponse> CreateUserAsync(CreateUser user)
    {
        try
        {
            return await _userClient.CreateUserAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {UserEmail}", user.Email);
            throw;
        }
    }

    public async Task UpdateUserAsync(Guid id, UpdateUser user)
    {
        try
        {
            await _userClient.UpdateUserAsync(id, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", id);
            throw;
        }
    }

    public async Task DeleteUserAsync(Guid id)
    {
        try
        {
            await _userClient.DeleteUserAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", id);
            throw;
        }
    }
}