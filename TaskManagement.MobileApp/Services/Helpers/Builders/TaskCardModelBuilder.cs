using TaskManagement.DTO.Office.User.Task;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.Services.Helpers.Builders;

public class TaskCardModelBuilder
{
    private readonly UserTaskResponseDto _taskItem;

    private TaskCardModelBuilder(UserTaskResponseDto taskItem)
    {
        _taskItem = taskItem;
    }

    public static TaskCardModelBuilder From(UserTaskResponseDto taskItem)
    {
        return new TaskCardModelBuilder(taskItem);
    }

    public TaskCardModel BuildTaskCard()
    {
        return new TaskCardModel(
            _taskItem.User.FirstName.ToCharArray().First(),
            _taskItem.Title,
            _taskItem.LinkedObject?.Id,
            _taskItem.DueDate
        );
    }
}