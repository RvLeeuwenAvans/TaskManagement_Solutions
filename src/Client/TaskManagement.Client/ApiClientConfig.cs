using System.Text.Json;

namespace TaskManagement.Client;

public class ApiClientConfig
{
    public required string BaseUrl { get; set; }
    
    /**
     * dev only, only used to set an admin token.
     */
    public string? AuthToken { get; set; }
    
    public JsonSerializerOptions JsonOptions { get; set; } = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };
}