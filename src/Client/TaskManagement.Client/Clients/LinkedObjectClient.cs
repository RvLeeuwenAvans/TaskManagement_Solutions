using TaskManagement.DTO.Office.User.Task.LinkedObject;

namespace TaskManagement.Client.Clients;

public class LinkedObjectClient(HttpClient httpClient, ApiClientConfig config) : BaseClient(httpClient, config)
{
    public async Task<LinkedObjectResponse> CreateLinkedObjectAsync(CreateLinkedObject linkedObject,
        CancellationToken cancellationToken = default)
    {
        return await PostAsync<CreateLinkedObject, LinkedObjectResponse>("/api/LinkedObject", linkedObject, cancellationToken);
    }
}