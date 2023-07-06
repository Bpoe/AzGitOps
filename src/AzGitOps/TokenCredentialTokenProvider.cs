namespace Microsoft.Azure.GitOps;

using global::Azure.Core;
using Microsoft.Rest;
using System.Net.Http.Headers;

public class TokenCredentialTokenProvider : ITokenProvider
{
    private const string Scheme = "Bearer";
    private readonly TokenCredential credential;
    private readonly string[] scopes;

    public TokenCredentialTokenProvider(TokenCredential credential, string[] scopes)
    {
        this.credential = credential ?? throw new ArgumentNullException(nameof(credential));
        this.scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
    }

    public async Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken = default)
    {
        var context = new TokenRequestContext(this.scopes);
        var token = await this.credential.GetTokenAsync(context, cancellationToken);
        return new AuthenticationHeaderValue(Scheme, token.Token);
    }
}
