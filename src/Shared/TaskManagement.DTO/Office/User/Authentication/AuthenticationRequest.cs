namespace TaskManagement.DTO.Office.User.Authentication;

public record AuthenticationRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}