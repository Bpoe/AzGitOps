namespace Microsoft.Azure.GitOps;

using AzGitOps.Models;
using global::Azure.Core;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using Newtonsoft.Json;

public class DeploymentOperator
{
    private readonly ArmResourceClientFactory armResourceClientFactory;
    private readonly TokenCredential tokenCredential;
    private readonly TemplateRetriever templateRetriever;
    private readonly ILogger<DeploymentOperator> logger;

    public DeploymentOperator(
        ArmResourceClientFactory armResourceClientFactory,
        TokenCredential tokenCredential,
        TemplateRetriever templateRetriever,
        ILogger<DeploymentOperator> logger)
    {
        this.armResourceClientFactory = armResourceClientFactory ?? throw new ArgumentNullException(nameof(armResourceClientFactory));
        this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));
        this.templateRetriever = templateRetriever ?? throw new ArgumentNullException(nameof(templateRetriever));
        this.logger = logger;
    }

    public virtual async Task Reconcile(string desiredStateResourceId)
    {
        this.logger?.LogInformation("Checking compliance for {source}...", desiredStateResourceId);

        // Arrange
        var credentials = await this.GetCredentials(desiredStateResourceId);

        var armClient = this.armResourceClientFactory.GetArmResourceClient<DeploymentOperator>(credentials);
        var rawResource = await armClient.GetAsync(desiredStateResourceId, "2022-05-04", true);
        if (rawResource is null)
        {
            this.logger?.LogWarning("Resource was not found!");
            return;
        }
        
        var desiredState = rawResource.ToObject<DesiredStateResource>();
        desiredState.Properties.Status = new DesiredStateStatus()
        {
            Status = "Pending",
        };

        await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));

        var template = await templateRetriever.GetTemplate(desiredState.Properties.TemplatePath) ?? throw new ArgumentNullException("Template was null!");
        var parameters = await templateRetriever.GetTemplate(desiredState.Properties.ParametersPath) ?? throw new ArgumentNullException("Parameters file was null!");
        
        var scopeId = new ResourceIdentifier(desiredState.Properties.Scope);
        var client = await this.GetResourceManagementClient(credentials, scopeId.SubscriptionId);

        // Test
        var changesNeeded = await this.Test(client, desiredState, template.ToString(), parameters.ToString());
        var compliant = !changesNeeded.Any();
        var complianceState = compliant ? "Compliant" : "Noncompliant";
        this.logger?.LogInformation("Compliance State: {complianceState}", complianceState);

        // TODO: Update status of resource
        desiredState.Properties.Status.Status = complianceState;
        if (compliant)
        {
            desiredState.Properties.Status.AssessmentEndedAt = DateTime.UtcNow;
        }

        await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));

        if (compliant)
        {
            this.logger?.LogInformation("No deployment needed.");
            return;
        }

        // Set
        var deploymentResult = await this.Set(client, desiredState, template.ToString(), parameters.ToString());

        desiredState.Properties.Status.Status = string.Equals("Succeeded", deploymentResult.Properties.ProvisioningState, StringComparison.InvariantCultureIgnoreCase)
            ? "CompliantCorrected"
            : "NoncompliantCorrectionFailed";

        await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));
    }

    private async Task<IEnumerable<WhatIfChange>> Test(IResourceManagementClient client, DesiredStateResource desiredState, string template, string parameters)
    {
        var deploymentWhatIf = new DeploymentWhatIf
        {
            Properties = new DeploymentWhatIfProperties
            {
                Template = template,
                Parameters = parameters,
                Mode = DeploymentMode.Incremental,
            },
        };

        var scopeId = new ResourceIdentifier(desiredState.Properties.Scope);
        var resourceGroupName = scopeId.ResourceGroupName;

        this.logger?.LogInformation("Starting WhatIf operation...");
        var whatIfResponse = await client.Deployments.WhatIfAsync(resourceGroupName, desiredState.Name, deploymentWhatIf);
        var changesNeeded = whatIfResponse
            .Changes
            .Where(c => c.ChangeType != ChangeType.Ignore && c.ChangeType != ChangeType.NoChange);
        this.logger?.LogInformation("WhatIf operation completed. Count of changes needed: {count}", changesNeeded.Count());

        return changesNeeded;
    }

    private async Task<DeploymentExtended> Set(IResourceManagementClient client, DesiredStateResource desiredState, string template, string parameters)
    {
        var deployment = new Deployment
        {
            Properties = new DeploymentProperties
            {
                Template = template.ToString(),
                Parameters = parameters.ToString(),
                Mode = DeploymentMode.Incremental,
            },
        };
        this.logger?.LogInformation("Starting Deployment operation...");
        var deploymentResult = await client.Deployments.CreateOrUpdateAtScopeAsync(desiredState.Properties.Scope, desiredState.Name, deployment);
        this.logger?.LogInformation("Deployment operation completed. Status: {status}", deploymentResult.Properties.ProvisioningState);

        return deploymentResult;
    }

    private Task<ServiceClientCredentials> GetCredentials(string targetResourceId)
    {
        var provider = new TokenCredentialTokenProvider(this.tokenCredential, new[] { "https://management.azure.com/.default" });
        return Task.FromResult<ServiceClientCredentials>(new TokenCredentials(provider));
    }

    private Task<IResourceManagementClient> GetResourceManagementClient(ServiceClientCredentials credentials, string subscriptionId)
    {
        var client = new ResourceManagementClient(credentials)
        {
            SubscriptionId = subscriptionId
        };

        return Task.FromResult<IResourceManagementClient>(client);
    }
}
