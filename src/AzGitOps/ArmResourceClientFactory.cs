namespace Microsoft.Azure.GitOps;

using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System.Net.Http;

public class ArmResourceClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<ArmResourceClient> logger;

    public ArmResourceClientFactory(IHttpClientFactory httpClientFactory, ILogger<ArmResourceClient> logger)
    {
        this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this.logger = logger;
    }

    public ArmResourceClient GetArmResourceClient<TType>(ServiceClientCredentials serviceClientCredentials)
    {
        var client = this.httpClientFactory.CreateClient("ResourceProvider");
        return new ArmResourceClient(client, serviceClientCredentials, this.logger);
    }
}
