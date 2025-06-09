using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskManagement.MobileApp.ViewModels.messages;

public class TaskEditedMessage(bool value) : ValueChangedMessage<bool>(value);