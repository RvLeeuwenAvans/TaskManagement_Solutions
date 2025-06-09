namespace TaskManagement.MobileApp.Models.Collections;

public record UserItem(
    Guid Id,
    string Firstname,
    string Lastname)
{
    public override string ToString() => $"{Firstname} {Lastname}";
}