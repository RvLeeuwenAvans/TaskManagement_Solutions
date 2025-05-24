namespace TaskManagement.DTO.Office.User.Authentication;

public record AuthenticationResponseDto
{
  public required string Token { get; init; }
}