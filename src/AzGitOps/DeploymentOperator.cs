namespace Microsoft.Azure.GitOps;

using AzGitOps.Models;
using global::Azure.Core;
using Newtonsoft.Json;
using WhatIf.Model;

public class DeploymentOperator
{
    private readonly ArmResourceClientFactory armResourceClientFactory;
    private readonly TokenCredential tokenCredential;
    private readonly ILogger<DeploymentOperator> logger;

    public DeploymentOperator(
        ArmResourceClientFactory armResourceClientFactory,
        TokenCredential tokenCredential,
        ILogger<DeploymentOperator> logger)
    {
        this.armResourceClientFactory = armResourceClientFactory ?? throw new ArgumentNullException(nameof(armResourceClientFactory));
        this.tokenCredential = tokenCredential ?? throw new ArgumentNullException(nameof(tokenCredential));
        this.logger = logger;
    }

    public virtual async Task Reconcile(string desiredStateResourceId)
    {
        this.logger?.LogInformation("Checking compliance for {source}...", desiredStateResourceId);

        var resource = new ResourceIdentifier(desiredStateResourceId);
        var armClient = this.armResourceClientFactory.GetArmResourceClient<DeploymentOperator>(this.tokenCredential);

        // Get desired state object
        var desiredStateContent = await File.ReadAllTextAsync("deploymentWhatIfRequest.json");
        var desiredState = JsonConvert.DeserializeObject<DesiredStateResource>(desiredStateContent);
        desiredState.Name = resource.Name;
        desiredState.Properties.Scope = $"/subscriptions/{resource.SubscriptionId}/resourceGroups/{resource.ResourceGroupName}";

        //var rawResource = await armClient.GetAsync(desiredStateResourceId, "2022-05-04", true);
        //if (rawResource is null)
        //{
        //    this.logger?.LogWarning("Resource was not found!");
        //    return;
        //}

        //var desiredState = rawResource.ToObject<DesiredStateResource>();
        //desiredState.Properties.Status = new DesiredStateStatus()
        //{
        //    Status = "Pending",
        //};

        //await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));

        // Test
        var changesNeeded = await this.Test(armClient, desiredState);
        var compliant = !changesNeeded.Any();
        var complianceState = compliant ? "Compliant" : "Noncompliant";
        this.logger?.LogInformation("Compliance State: {complianceState}", complianceState);

        //// TODO: Update status of resource
        //desiredState.Properties.Status.Status = complianceState;
        //if (compliant)
        //{
        //    desiredState.Properties.Status.AssessmentEndedAt = DateTime.UtcNow;
        //}

        //await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));

        if (compliant)
        {
            this.logger?.LogInformation("No deployment needed.");
            return;
        }

        // Set
        //var deploymentResult = await this.Set(armClient, desiredState, template.ToString(), parameters.ToString());

        //desiredState.Properties.Status.Status = string.Equals("Succeeded", deploymentResult.Properties.ProvisioningState, StringComparison.InvariantCultureIgnoreCase)
        //    ? "CompliantCorrected"
        //    : "NoncompliantCorrectionFailed";

        //await armClient.PutAsync(desiredStateResourceId, "2022-05-04", JsonConvert.SerializeObject(desiredState));
    }

    private async Task<IEnumerable<WhatIfChange>> Test(ArmResourceClient armClient, DesiredStateResource desiredState)
    {
        this.logger?.LogInformation("Starting WhatIf operation...");

        var request = new DeploymentWhatIf
        {
            Properties = new DeploymentWhatIfProperties
            {
                Template = desiredState.Properties.Template,
                TemplateLink = desiredState.Properties.TemplateLink,
                Parameters = desiredState.Properties.Parameters,
                ParametersLink = desiredState.Properties.ParametersLink,
                Mode = desiredState.Properties.Mode,
                ExpressionEvaluationOptions = desiredState.Properties.ExpressionEvaluationOptions,
                DebugSetting = desiredState.Properties.DebugSetting,
                OnErrorDeployment = desiredState.Properties.OnErrorDeployment,
            }
        };

        var responseRaw = await armClient.PostAsync(
            $"{desiredState.Properties.Scope}/providers/Microsoft.Resources/deployments/{desiredState.Name}/whatIf",
            "2021-04-01",
            JsonConvert.SerializeObject(request));

        var whatIfResponse = responseRaw.ToObject<WhatIfOperationResult>();
        var changesNeeded = whatIfResponse
            .Properties
            .Changes
            .Where(c => !c.ChangeType.EqualsIgnoreCase("Ignore") && !c.ChangeType.EqualsIgnoreCase("NoChange"));
        this.logger?.LogInformation("WhatIf operation completed. Count of changes needed: {count}", changesNeeded.Count());

        return changesNeeded;
    }

    private async Task Set(ArmResourceClient client, DesiredStateResource desiredState)
    {
        var deployment = new Deployment
        {
            Properties = new DeploymentProperties
            {
                Template = desiredState.Properties.Template,
                TemplateLink = desiredState.Properties.TemplateLink,
                Parameters = desiredState.Properties.Parameters,
                ParametersLink = desiredState.Properties.ParametersLink,
                Mode = desiredState.Properties.Mode,
                ExpressionEvaluationOptions = desiredState.Properties.ExpressionEvaluationOptions,
                DebugSetting = desiredState.Properties.DebugSetting,
                OnErrorDeployment = desiredState.Properties.OnErrorDeployment,
            },
        };

        this.logger?.LogInformation("Starting Deployment operation...");
        //var deploymentResult = await client.Deployments.CreateOrUpdateAtScopeAsync(desiredState.Properties.Scope, desiredState.Name, deployment);
        //this.logger?.LogInformation("Deployment operation completed. Status: {status}", deploymentResult.Properties.ProvisioningState);

        //return deploymentResult;
    }
}
