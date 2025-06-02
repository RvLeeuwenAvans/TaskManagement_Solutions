using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services.Authentication;
using TaskManagement.MobileApp.Services.Helpers.Builders;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services;

//todo: remove auth service
public class TaskService(IUserContext userContext, ITaskRepository repository, AuthService authService)
{
    public async Task<List<UserTaskCardItem>> GetUserTasks()
    {
        await authService.AuthenticateUser("jane.smith@example.com", "hashedpassword23");

        var userTasks = await repository.GetTasksAsync(userContext.UserId);
        return userTasks.Select(task => TaskCardModelBuilder.From(task).Build()).ToList();
    }

    public async Task<bool> CloseTaskAsync(Guid taskId)
    {
        try
        {
            await repository.CloseTasksAsync(taskId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}