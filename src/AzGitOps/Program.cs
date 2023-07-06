using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.GitOps;

Console.WriteLine("Azure GitOps Operator\n");

Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .ConfigureWebJobs((context, b) =>
    {
        b.AddAzureStorageQueues();
    })
    .Build()
    .Run();

static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
{
    services
        .AddSingleton(new DefaultAzureCredentialOptions { ExcludeAzureCliCredential = true, ExcludeManagedIdentityCredential = true, })
        .AddSingleton<TokenCredential, DefaultAzureCredential>(p =>
        {
            var o = p.GetRequiredService<DefaultAzureCredentialOptions>();
            return new DefaultAzureCredential(o);
        })
        .AddSingleton<DeploymentOperator>()
        .AddSingleton<TemplateRetriever>()
        .AddSingleton<ArmResourceClientFactory>();

    if (!hostBuilderContext.HostingEnvironment.IsDevelopment())
    {
        services.AddHttpClient("ResourceProvider", h => h.BaseAddress = new Uri("https://management.azure.com"));
    }
    else
    {
        services.AddHttpClient("ResourceProvider", h => h.BaseAddress = new Uri("http://localhost:5000"));
    }
}
