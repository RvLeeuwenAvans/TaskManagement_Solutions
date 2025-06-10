using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskManagement.MobileApp.ViewModels.messages;

public class TypeFilterSelectedMessage(TaskTypeFilter value) : ValueChangedMessage<TaskTypeFilter>(value);