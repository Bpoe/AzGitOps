namespace Microsoft.Azure.GitOps;

using global::Azure.Core;
using global::Azure.Storage.Blobs;
using Newtonsoft.Json.Linq;

public class TemplateRetriever
{
    private readonly TokenCredential tokenCredential;

    public TemplateRetriever(TokenCredential tokenCredential)
    {
        this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));
    }

    public async Task<JObject?> GetTemplate(string template)
    {
        var blobUri = new Uri(template);
        var blobClient = new BlobClient(blobUri, this.tokenCredential);

        var content = await blobClient.DownloadContentAsync();
        return JObject.Parse(content.Value.Content.ToString());
    }
}