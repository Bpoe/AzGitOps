namespace Microsoft.Azure.GitOps;

using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net;

public class ArmResourceClient
{
    private readonly IDictionary<string, JContainer> cache = new ConcurrentDictionary<string, JContainer>(StringComparer.InvariantCultureIgnoreCase);
    private readonly HttpClient httpClient;
    private readonly ServiceClientCredentials serviceClientCredentials;
    private readonly ILogger<ArmResourceClient> logger;

    public ArmResourceClient(HttpClient httpClient, ServiceClientCredentials serviceClientCredentials, ILogger<ArmResourceClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(httpClient.BaseAddress, nameof(httpClient.BaseAddress));

        this.httpClient = httpClient;
        this.serviceClientCredentials = serviceClientCredentials ?? throw new ArgumentNullException(nameof(serviceClientCredentials));
        this.logger = logger;
    }

    protected ArmResourceClient()
    {
    }

    public virtual async Task<JContainer> GetAsync(string resourceId, string apiVersion, bool refreshCache = false, CancellationToken cancellationToken = default)
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

    public virtual async Task<JContainer> PostAsync(string path, string apiVersion, string body, CancellationToken cancellationToken = default)
        => await this.PostAsync(path, apiVersion, body, null, cancellationToken);

    public virtual async Task<JContainer> PostAsync(string path, string apiVersion, string body, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
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

    public virtual async Task<JContainer> PutAsync(string path, string apiVersion, string body, CancellationToken cancellationToken = default)
    {
        using var request = await this.GetRequestMessage(HttpMethod.Put, path, apiVersion, cancellationToken);
        request.Content = new StringContent(body);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return await this.GetResultForRequest(request, cancellationToken);
    }

    public virtual async Task<JContainer> DeleteAsync(string path, string apiVersion, CancellationToken cancellationToken = default)
    {
        using var request = await this.GetRequestMessage(HttpMethod.Delete, path, apiVersion, cancellationToken);
        return await this.GetResultForRequest(request, cancellationToken);
    }

    private async Task<HttpRequestMessage> GetRequestMessage(HttpMethod httpMethod, string resourceId, string apiVersion, CancellationToken cancellationToken = default)
    {
        var uri = GetResourceUri(resourceId, apiVersion);

        var request = new HttpRequestMessage(httpMethod, uri);

        // <Workaround>
        //  AzureCredentials.ProcessHttpRequestAsync has an issue where it throws if the Uri is a relative Uri
        //  (which is what we do). So we will check if its relative and temporarily set a new absolute Uri and
        //  then swap back the original Uri after the call.
        //  https://github.com/Azure/azure-libraries-for-net/issues/1257
        var originalRequestUri = request.RequestUri;
        if (!originalRequestUri.IsAbsoluteUri)
        {
            request.RequestUri = new Uri(this.httpClient.BaseAddress, originalRequestUri);
        }

        await this.serviceClientCredentials.ProcessHttpRequestAsync(request, cancellationToken);

        request.RequestUri = originalRequestUri;
        // </Workaround>

        return request;
    }

    private async Task<JContainer?> GetResultForRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var result = await this.httpClient.SendAsync(request, cancellationToken);
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
