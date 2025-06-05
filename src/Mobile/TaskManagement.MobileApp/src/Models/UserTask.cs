using TaskManagement.MobileApp.Models.Collections;

namespace TaskManagement.MobileApp.Models;

public record UserTask(
    string Title,
    string? Description,
    DateTime DueDate,
    UserItem AssignedUser,
    LinkedObjectItem? LinkedObject);