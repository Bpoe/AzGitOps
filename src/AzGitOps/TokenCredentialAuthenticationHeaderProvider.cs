namespace Microsoft.Azure.GitOps;

using System.Net.Http.Headers;
using global::Azure.Core;

public class TokenCredentialAuthenticationHeaderProvider : IAuthenticationHeaderProvider
{
    private const string Scheme = "Bearer";
    private const string Default = ".default";

    private readonly TokenCredential tokenCredential;
    private readonly AzureEnvironment azureEnvironment;

    public TokenCredentialAuthenticationHeaderProvider(TokenCredential tokenCredential, AzureEnvironment azureEnvironment)
    {
        this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));
        this.azureEnvironment = azureEnvironment ?? throw new ArgumentNullException(nameof(azureEnvironment));
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken = default)
    {
        var context = new TokenRequestContext(new[] { this.azureEnvironment.ResourceManagerEndpoint + Default });
        var token = await this.tokenCredential.GetTokenAsync(context, cancellationToken);
        return new AuthenticationHeaderValue(Scheme, token.Token);
    }
}
