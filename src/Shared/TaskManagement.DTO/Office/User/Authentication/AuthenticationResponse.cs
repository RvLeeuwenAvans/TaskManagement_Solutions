namespace TaskManagement.DTO.Office.User.Authentication;

public record AuthenticationResponse
{
  public required string Token { get; init; }
}