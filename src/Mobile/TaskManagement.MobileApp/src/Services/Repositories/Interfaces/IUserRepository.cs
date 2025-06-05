using TaskManagement.DTO.Office.User;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserResponse> GetUserById(Guid userId);
    
    Task<List<UserResponse>> GetUsersByOfficeAsync(Guid officeId);
}