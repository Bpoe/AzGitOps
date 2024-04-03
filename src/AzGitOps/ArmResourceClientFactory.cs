namespace Microsoft.Azure.GitOps;

using System.Net.Http;
using global::Azure.Core;
using Microsoft.Extensions.Logging;

public class ArmResourceClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly AzureEnvironment azureEnvironment;
    private readonly ILogger<ArmResourceClient> logger;

    public ArmResourceClientFactory(IHttpClientFactory httpClientFactory, AzureEnvironment azureEnvironment, ILogger<ArmResourceClient> logger)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this.azureEnvironment = azureEnvironment ?? throw new ArgumentNullException(nameof(azureEnvironment));
        this.logger = logger;
    }

    public ArmResourceClient GetArmResourceClient<TType>(TokenCredential tokenCredential)
    {
        var client = this.httpClientFactory.CreateClient("ResourceProvider");
        return new ArmResourceClient(client, tokenCredential, this.azureEnvironment, this.logger);
    }
}
