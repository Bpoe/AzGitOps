namespace Microsoft.Azure.GitOps;
using System.Net.Http.Headers;

public interface IAuthenticationHeaderProvider
{
    Task<AuthenticationHeaderValue> GetAuthenticationHeaderAsync(CancellationToken cancellationToken);
}
