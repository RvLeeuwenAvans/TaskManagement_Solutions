using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.MobileApp.Models.Collections;

public record UserTaskCardItem(
    Guid Id,
    char CreatorInitial,
    string Title,
    LinkedObjectResponse? LinkedObjectResponse,
    DateTime DueDate
    );