namespace Microsoft.Azure.GitOps;

public class AzureEnvironment
{
    public string Name { get; set; }

    public string AuthenticationEndpoint { get; set; }

    public string ResourceManagerEndpoint { get; set; }

    public string GraphEndpoint { get; set; }

    public string ManagementEndpoint { get; set; }

    public string StorageEndpointSuffix { get; set; }

    public string KeyVaultSuffix { get; set; }

    public static readonly AzureEnvironment AzureGlobalCloud = new()
    {
        Name = "AzureGlobalCloud",
        AuthenticationEndpoint = "https://login.microsoftonline.com/",
        ResourceManagerEndpoint = "https://management.azure.com/",
        ManagementEndpoint = "https://management.core.windows.net/",
        GraphEndpoint = "https://graph.windows.net/",
        StorageEndpointSuffix = "core.windows.net",
        KeyVaultSuffix = "vault.azure.net"
    };

    public static readonly AzureEnvironment AzureChinaCloud = new()
    {
        Name = "AzureChinaCloud",
        AuthenticationEndpoint = "https://login.chinacloudapi.cn/",
        ResourceManagerEndpoint = "https://management.chinacloudapi.cn/",
        ManagementEndpoint = "https://management.core.chinacloudapi.cn/",
        GraphEndpoint = "https://graph.chinacloudapi.cn/",
        StorageEndpointSuffix = "core.chinacloudapi.cn",
        KeyVaultSuffix = "vault.azure.cn"
    };

    public static readonly AzureEnvironment AzureUSGovernment = new()
    {
        Name = "AzureUSGovernment",
        AuthenticationEndpoint = "https://login.microsoftonline.us/",
        ResourceManagerEndpoint = "https://management.usgovcloudapi.net/",
        ManagementEndpoint = "https://management.core.usgovcloudapi.net/",
        GraphEndpoint = "https://graph.windows.net/",
        StorageEndpointSuffix = "core.usgovcloudapi.net",
        KeyVaultSuffix = "vault.usgovcloudapi.net"
    };

    public static readonly AzureEnvironment AzureGermanCloud = new()
    {
        Name = "AzureGermanCloud",
        AuthenticationEndpoint = "https://login.microsoftonline.de/",
        ResourceManagerEndpoint = "https://management.microsoftazure.de/",
        ManagementEndpoint = "https://management.core.cloudapi.de/",
        GraphEndpoint = "https://graph.cloudapi.de/",
        StorageEndpointSuffix = "core.cloudapi.de",
        KeyVaultSuffix = "vault.microsoftazure.de"
    };

    public static readonly IEnumerable<AzureEnvironment> KnownEnvironments = new[]
    {
        AzureGlobalCloud,
        AzureChinaCloud,
        AzureUSGovernment,
        AzureGermanCloud,
    };

    public static AzureEnvironment? FromName(string name)
        => KnownEnvironments.FirstOrDefault((AzureEnvironment env) => string.Equals(env.Name, name, StringComparison.OrdinalIgnoreCase));
}