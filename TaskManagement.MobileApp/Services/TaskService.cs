using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Repositories.Interfaces;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Helpers.Builders;

namespace TaskManagement.MobileApp.Services;

//todo: remove auth service
public class TaskService(IUserContext userContext, ITaskRepository repository, AuthService authService)
{
    public async Task<List<TaskCardModel>> GetUserTasks()
    {
        await authService.AuthenticateUser("jane.smith@example.com", "hashedpassword23");
        
        var userTasks = await repository.GetTasksAsync(userContext.UserId);
        return userTasks.Select(task => TaskCardModelBuilder.From(task).Build()).ToList();
    }
}