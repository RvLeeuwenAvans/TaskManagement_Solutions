using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Models;
using TaskManagement.MobileApp.Models.Collections;

namespace TaskManagement.MobileApp.Services.Helpers.Builders;

public class TaskCardModelBuilder
{
    private readonly UserTaskResponse _taskItem;

    private TaskCardModelBuilder(UserTaskResponse taskItem)
    {
        _taskItem = taskItem;
    }

    public static TaskCardModelBuilder From(UserTaskResponse taskItem)
    {
        return new TaskCardModelBuilder(taskItem);
    }

    public UserTaskCardItem Build()
    {
        return new UserTaskCardItem(
            _taskItem.Id,
            _taskItem.User.FirstName.ToCharArray().First(),
            _taskItem.Title,
            _taskItem.LinkedObject,
            _taskItem.DueDate
        );
    }
}