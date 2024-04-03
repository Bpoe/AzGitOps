namespace Microsoft.Azure.GitOps;

using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net;
using global::Azure.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ArmResourceClient
{
    private readonly IDictionary<string, JContainer?> cache = new ConcurrentDictionary<string, JContainer?>(StringComparer.InvariantCultureIgnoreCase);
    private readonly HttpClient httpClient;
    private readonly IAuthenticationHeaderProvider authenticationHeaderProvider;
    private readonly ILogger<ArmResourceClient> logger;

    public ArmResourceClient(HttpClient httpClient, TokenCredential tokenCredential, AzureEnvironment azureEnvironment, ILogger<ArmResourceClient> logger)
    {
        ArgumentNullException.ThrowIfNull(tokenCredential, nameof(tokenCredential));
        ArgumentNullException.ThrowIfNull(azureEnvironment, nameof(azureEnvironment));

        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.authenticationHeaderProvider = new TokenCredentialAuthenticationHeaderProvider(tokenCredential, azureEnvironment);
    }

    public virtual async Task<JContainer?> GetAsync(string resourceId, string apiVersion, bool refreshCache = false, CancellationToken cancellationToken = default)
    {
        var key = resourceId + "?api-version=" + apiVersion;
        if (!refreshCache && this.cache.TryGetValue(key, out var cacheResource))
        {
            return cacheResource;
        }

        using var request = await this.GetRequestMessage(HttpMethod.Get, resourceId, apiVersion, cancellationToken);
        var result = await this.GetResultForRequest(request, cancellationToken);
        this.cache[key] = result;
        return result;
    }

    public virtual async Task<JContainer?> PostAsync(string path, string apiVersion, string body, CancellationToken cancellationToken = default)
        => await this.PostAsync(path, apiVersion, body, null, cancellationToken);

    public virtual async Task<JContainer?> PostAsync(string path, string apiVersion, string body, IDictionary<string, string>? headers, CancellationToken cancellationToken = default)
    {
        using var request = await this.GetRequestMessage(HttpMethod.Post, path, apiVersion, cancellationToken);
        request.Content = new StringContent(body);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        if (headers != null)
        {
            foreach (var h in headers)
            {
                request.Content.Headers.Add(h.Key, h.Value);
            }
        }

        return await this.GetResultForRequest(request, cancellationToken);
    }

    public virtual async Task<JContainer?> PutAsync(string path, string apiVersion, string body, CancellationToken cancellationToken = default)
    {
        using var request = await this.GetRequestMessage(HttpMethod.Put, path, apiVersion, cancellationToken);
        request.Content = new StringContent(body);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return await this.GetResultForRequest(request, cancellationToken);
    }

    public virtual async Task<JContainer?> DeleteAsync(string path, string apiVersion, CancellationToken cancellationToken = default)
    {
        using var request = await this.GetRequestMessage(HttpMethod.Delete, path, apiVersion, cancellationToken);
        return await this.GetResultForRequest(request, cancellationToken);
    }

    private async Task<HttpRequestMessage> GetRequestMessage(HttpMethod httpMethod, string resourceId, string apiVersion, CancellationToken cancellationToken = default)
    {
        var uri = GetResourceUri(resourceId, apiVersion);

        var request = new HttpRequestMessage(httpMethod, uri);
        request.Headers.Authorization = await this.authenticationHeaderProvider.GetAuthenticationHeaderAsync(cancellationToken);

        return request;
    }

    private async Task<JContainer?> GetResultForRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var result = await SendAndFollowLocation(this.httpClient, request, cancellationToken);
        if (result.StatusCode == HttpStatusCode.NotFound || result.StatusCode == HttpStatusCode.Forbidden)
        {
            this.logger?.LogWarning("The request {requestUri} returned status code: {statusCode}", request.RequestUri, result.StatusCode);
            return null;
        }

        var responseBody = await result
            .EnsureSuccessStatusCode() // If the response was not successful, throw
            .Content
            .ReadAsStringAsync();

        try
        {
            return string.IsNullOrWhiteSpace(responseBody) ? null : JObject.Parse(responseBody);
        }
        catch (JsonReaderException ex)
        {
            this.logger?.LogWarning(ex, "The request {requestUri} returned a response that was not parsable JSON: {response}", request.RequestUri, responseBody);
            return null;
        }
    }

    private static async Task<HttpResponseMessage> SendAndFollowLocation(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await client.SendAsync(request, cancellationToken);
        while (response.StatusCode == HttpStatusCode.Accepted)
        {
            if (response.Headers.RetryAfter is not null && response.Headers.RetryAfter.Delta.HasValue)
            {
                await Task.Delay(response.Headers.RetryAfter.Delta.Value, cancellationToken);
            }

            var operationRequest = new HttpRequestMessage(HttpMethod.Get, response.Headers.Location);
            operationRequest.Headers.Authorization = request.Headers.Authorization;
            response = await client.SendAsync(operationRequest, cancellationToken);
        }

        return response;
    }

    private static Uri GetResourceUri(string path, string apiVersion)
    {
        var uriBuilder = new UriBuilder
        {
            Path = path,
            Query = "api-version=" + apiVersion
        };

        return new Uri(uriBuilder.Uri.PathAndQuery, UriKind.Relative);
    }
}
