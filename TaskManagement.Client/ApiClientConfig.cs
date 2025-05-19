namespace TaskManagement.Client;

public class ApiClientConfig
{
    public string BaseUrl { get; init; } = string.Empty;
    
    /**
     * dev only, only used to set an admin token.
     */
    public string? AuthToken { get; init; }
}