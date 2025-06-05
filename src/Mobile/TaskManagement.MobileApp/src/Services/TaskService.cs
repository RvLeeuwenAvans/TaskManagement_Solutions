using TaskManagement.DTO.Office.User.Task;
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
    public async Task<List<UserTaskCardItem>> GetUserTasksAsync()
    {
        await authService.AuthenticateUser("jane.smith@example.com", "hashedpassword23");

        var userTasks = await repository.GetTasksAsync(userContext.UserId);
        return userTasks.Select(task => TaskCardModelBuilder.From(task).Build()).ToList();
    }

    public async Task<UserTaskResponse> GetTaskByIdAsync(Guid taskId)
    {
        return await repository.GetTaskAsync(taskId);
    }

    public async Task CreateTaskAsync(UserTask task, UserItem taskCreator)
    {
        try
        {
            await repository.CreateTaskAsync(new CreateUserTask
            {
                Title = task.Title,
                Description = task.Description,
                UserId = task.AssignedUser.Id,
                DueDate = task.DueDate,
                LinkedObjectId = task.LinkedObject?.Id,
                CreatorName = taskCreator.Firstname
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    public async Task UpdateTaskAsync(UserTask task, Guid taskId)
    {
        try
        {
            await repository.UpdateTaskAsync(new UpdateUserTask
            {
                Id = taskId,
                Title = task.Title,
                Description = task.Description,
                UserId = task.AssignedUser.Id,
                DueDate = task.DueDate,
                LinkedObjectId = task.LinkedObject?.Id
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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