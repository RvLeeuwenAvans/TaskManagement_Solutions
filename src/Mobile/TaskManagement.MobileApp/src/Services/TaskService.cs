using TaskManagement.DTO.Office.User.Task;
using TaskManagement.DTO.Office.User.Task.Note;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services.Helpers.Builders;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services;

public class TaskService(
    IUserContext userContext,
    ITaskRepository taskRepository,
    INoteRepository noteRepository,
    LinkedObjectService linkedObjectService)
{
    public async Task<List<UserTaskCardItem>> GetCurrentUserTasksAsync()
    {
        var userTasks = await taskRepository.GetTasksAsync(userContext.UserId);
        return userTasks.Select(task => TaskCardModelBuilder.From(task).Build()).ToList();
    }

    public async Task<UserTaskResponse> GetTaskByIdAsync(Guid taskId)
    {
        return await taskRepository.GetTaskAsync(taskId);
    }

    public async Task<List<NoteResponse>> GetNotesByTaskIdAsync(Guid taskId)
    {
        return await noteRepository.GetNotesAsync(taskId);
    }

    public async Task<bool> CreateNoteAsync(Guid taskId, string content)
    {
        try
        {
            await noteRepository.CreateNoteAsync(new CreateNote { TaskId = taskId, Content = content });
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteNoteIdAsync(Guid noteId)
    {
        try
        {
            await noteRepository.DeleteNoteAsync(noteId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> CreateTaskAsync(UserTask taskToCreate, UserItem taskCreator)
    {
        try
        {
            var createdTask = await taskRepository.CreateTaskAsync(new CreateUserTask
            {
                Title = taskToCreate.Title,
                Description = taskToCreate.Description,
                UserId = taskToCreate.AssignedUser.Id,
                DueDate = taskToCreate.DueDate,
                CreatorName = taskCreator.Firstname
            });

            if (taskToCreate.LinkedObject is null) return true;

            try
            {
                await linkedObjectService.CreateLinkedObjectAsync(createdTask, taskToCreate.LinkedObject);
            }
            catch (Exception e)
            {
                // Task is created, but linked object fails; still return true
                await Shell.Current.DisplayAlert("Warning", "The task was created, but linking the object failed.",
                    "OK");
                Console.WriteLine(e);
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> UpdateTaskAsync(UserTask task, Guid taskId)
    {
        try
        {
            await taskRepository.UpdateTaskAsync(new UpdateUserTask
            {
                Id = taskId,
                Title = task.Title,
                Description = task.Description,
                UserId = task.AssignedUser.Id,
                DueDate = task.DueDate,
                LinkedObjectId = task.LinkedObject?.Id
            });

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> CloseTaskAsync(Guid taskId)
    {
        try
        {
            await taskRepository.CloseTasksAsync(taskId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}