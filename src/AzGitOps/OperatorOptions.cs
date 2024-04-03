namespace Microsoft.Azure.GitOps;

public class OperatorOptions
{
    public Uri ResourceProviderEndpoint { get; set; } = new Uri("https://management.azure.com");

    public string AzureEnvironment { get; set; } = "AzureGlobalCloud";
}