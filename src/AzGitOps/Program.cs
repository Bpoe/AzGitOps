using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.GitOps;

Console.WriteLine("Azure GitOps Operator\n");

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

var resourceId = $"/subscriptions/d0efb362-cb15-4021-9b3b-8c107b937d4c/resourceGroups/cleanupservice/providers/Microsoft.Foo/bars/{Guid.NewGuid()}";
// "0fff0b88-539a-4eda-86e4-d507bfdd8928";
var deploymentOperator = host.Services.GetRequiredService<DeploymentOperator>();
await deploymentOperator.Reconcile(resourceId);

//Host
//    .CreateDefaultBuilder(args)
//    .ConfigureServices(ConfigureServices)
//    .ConfigureWebJobs((context, b) =>
//    {
//        b.AddAzureStorageQueues();
//    })
//    .Build()
//    .Run();

static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
{
    services
        .AddSingleton(hostBuilderContext.Configuration.Get<DefaultAzureCredentialOptions>() ?? new DefaultAzureCredentialOptions())
        .AddSingleton<TokenCredential, DefaultAzureCredential>(p =>
        {
            var o = p.GetRequiredService<DefaultAzureCredentialOptions>();
            return new DefaultAzureCredential(o);
        });

    services.AddSingleton(hostBuilderContext.Configuration.Get<OperatorOptions>() ?? new OperatorOptions());
    services.AddSingleton<DeploymentOperator>();
    services.AddSingleton<ArmResourceClientFactory>();
    services.AddSingleton(s =>
        AzureEnvironment.FromName(s.GetRequiredService<OperatorOptions>().AzureEnvironment) ?? AzureEnvironment.AzureGlobalCloud);

    services.AddHttpClient("ResourceProvider", (s, httpClient) =>
    {
        var options = s.GetRequiredService<OperatorOptions>();
        httpClient.BaseAddress = options.ResourceProviderEndpoint;
    });
}
