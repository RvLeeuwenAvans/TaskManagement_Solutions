# TaskManagement.Client

A .NET client library for interacting with the TaskManagement API.

## Setup

Register the client services in your application's dependency injection container:

```csharp
services.Configure<ApiClientConfig>(config => 
{
    config.BaseUrl = "https://your-api-url.com";
    config.AuthToken = "optional-dev-token"; // Development only
});

services.RegisterClients();
```
## Usage
Inject any of the available clients into your services:
```csharp
public class ExampleService
{
    private readonly UserAuthenticationClient _authClient;
    private readonly UserTaskClient _taskClient;

    public ExampleService(UserAuthenticationClient authClient, UserTaskClient taskClient)
    {
        _authClient = authClient;
        _taskClient = taskClient;
    }

    public async Task DoWorkAsync()
    { 
        // Authenticate - this sets the JWT token for all clients using the shared HttpClient
        await _authClient.AuthenticateUserAsync("user@example.com", "password");

        // Use other clients
        var tasks = await _taskClient.GetTasksByUserAsync(userId);
    }
} 
```
## Authentication
When using the default setup with services.RegisterClients(), all clients share the same HttpClient instance. This means that authenticating with the UserAuthenticationClient will set the JWT token for all other clients automatically.

To logout and remove the authentication token from all clients, call:
```csharp
_authClient.Logout();
```
## Available Clients
- UserAuthenticationClient - User authentication (optional if implementing custom authentication)
- UserClient - User management
- UserTaskClient - Task management
- NoteClient - Task notes
- RelationClient - Relations management
- OfficeClient - Office management
- InsurancePolicyClient - Insurance policies
- DamageClaimClient - Damage claims
> ## Note: 
> The UserAuthenticationClient is not required if you want to implement your own authentication scheme. You can set authentication tokens directly on any client.