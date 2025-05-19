using TaskManagement.DTO.Office.User.Authentication;

namespace TaskManagement.Client.Clients;

public class AuthenticationClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    /**
    *  Logs in with provided credentials and sets the JWT token.
    **/
    public async Task AuthenticateUserAsync(string email, string password)
    {
        var request = new AuthenticationDto
        {
            Email = email,
            Password = password
        };

        var response =
            await PostAsync<AuthenticationDto, AuthenticationResponseDto>("Authentication/user", request);

        SetAuthToken(response.Token);
    }

    /**
    * Unsets the current authentication token. Token will remain valid until it expires naturally tho.
    **/
    public void Logout() => SetAuthToken(null);
}