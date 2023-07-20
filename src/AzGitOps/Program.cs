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
        .AddSingleton(hostBuilderContext.Configuration.Get<DefaultAzureCredentialOptions>() ?? new DefaultAzureCredentialOptions())
        .AddSingleton<TokenCredential, DefaultAzureCredential>(p =>
        {
            var o = p.GetRequiredService<DefaultAzureCredentialOptions>();
            return new DefaultAzureCredential(o);
        })
        .AddSingleton(hostBuilderContext.Configuration.Get<OperatorOptions>() ?? new OperatorOptions())
        .AddSingleton<DeploymentOperator>()
        .AddSingleton<TemplateRetriever>()
        .AddSingleton<ArmResourceClientFactory>();


    services.AddHttpClient("ResourceProvider", (s, h) =>
    {
        var o = s.GetRequiredService<OperatorOptions>();
        h.BaseAddress = o.ResourceProviderEndpoint;
    });
}
