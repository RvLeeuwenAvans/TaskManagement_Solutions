using TaskManagement.DTO.Office.User;

namespace TaskManagement.CMS.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetUsersByOfficeAsync(Guid officeId);
    Task<UserResponse> GetUserByIdAsync(Guid id);
    Task<UserResponse> CreateUserAsync(CreateUser user);
    Task UpdateUserAsync(Guid id, UpdateUser user);
    Task DeleteUserAsync(Guid id);
}