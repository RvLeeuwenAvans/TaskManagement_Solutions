using TaskManagement.DTO.Office.User.Authentication;

namespace TaskManagement.Client.Clients;

public class UserAuthenticationClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    /**
    *  Logs in with provided credentials and sets the JWT token.
    **/
    public async Task<string> AuthenticateUserAsync(string email, string password)
    {
        var request = new AuthenticationRequest
        {
            Email = email,
            Password = password
        };

        var response =
            await PostAsync<AuthenticationRequest, AuthenticationResponse>("Authentication/user", request);

        SetAuthToken(response.Token);
        
        return response.Token;
    }

    /**
    * Unsets the current authentication token. Token will remain valid until it expires naturally tho.
    **/
    public void Logout() => SetAuthToken(null);
}