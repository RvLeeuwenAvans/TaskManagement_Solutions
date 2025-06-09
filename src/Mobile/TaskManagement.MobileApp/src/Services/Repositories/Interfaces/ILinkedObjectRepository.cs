using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces
{
    public interface ILinkedObjectRepository
    {
        Task<LinkedObjectResponse> CreateLinkedObjectAsync(CreateLinkedObject dto);
    }
}