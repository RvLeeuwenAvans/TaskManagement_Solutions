using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace TaskManagement.Client;

public abstract class BaseClient
{
    private readonly HttpClient _httpClient;

    // todo: make this configurable.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    protected BaseClient(HttpClient httpClient, ApiClientConfig config)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(config);

        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(config.BaseUrl);
        SetAuthToken(config.AuthToken);
    }

    protected void SetAuthToken(string? token) =>
        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token);

    protected async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await DeserializeOrThrowAsync<T>(response.Content, endpoint, cancellationToken);
    }

    protected async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await DeserializeOrThrowAsync<TResponse>(response.Content, endpoint, cancellationToken);
    }

    protected async Task<TResponse> PostAsync<TResponse>(
        string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(endpoint, null, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await DeserializeOrThrowAsync<TResponse>(response.Content, endpoint, cancellationToken);
    }

    protected async Task PostAsync<TRequest>(
        string endpoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, request, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    protected async Task<TResponse> PutAsync<TRequest, TResponse>(
        string endpoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, request, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await DeserializeOrThrowAsync<TResponse>(response.Content, endpoint, cancellationToken);
    }

    protected async Task PutAsync<TRequest>(
        string endpoint, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, request, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    protected async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private static async Task<T> DeserializeOrThrowAsync<T>(
        HttpContent content, string requestUri, CancellationToken cancellationToken)
    {
        var result = await content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken);
        if (result is not null) return result;
        
        var rawBody = await content.ReadAsStringAsync(cancellationToken);
        throw new InvalidOperationException(
            $"Deserialization failed for '{typeof(T).Name}' from '{requestUri}'. " +
            $"Raw response: '{rawBody}'");
    }
}
