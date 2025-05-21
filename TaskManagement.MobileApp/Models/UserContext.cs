using TaskManagement.MobileApp.Models.Interfaces;

namespace TaskManagement.MobileApp.Models;

public class UserContext : IUserContext
{
    public Guid UserId { get; set; }
}