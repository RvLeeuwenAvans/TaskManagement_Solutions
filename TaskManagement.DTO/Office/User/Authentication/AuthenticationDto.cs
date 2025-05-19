namespace TaskManagement.DTO.Office.User.Authentication;

public record AuthenticationDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}