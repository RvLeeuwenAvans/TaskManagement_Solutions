using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskManagement.MobileApp.ViewModels.messages;

public class TaskClosedMessage(bool value) : ValueChangedMessage<bool>(value);