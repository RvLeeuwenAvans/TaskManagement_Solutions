using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskManagement.MobileApp.ViewModels.messages;

public class TaskAddedMessage(bool value) : ValueChangedMessage<bool>(value);