namespace Microsoft.Azure.GitOps;

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

        // Arrange
        var armClient = this.armResourceClientFactory.GetArmResourceClient<DeploymentOperator>(this.tokenCredential);

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

        var scope = "/subscriptions/d0efb362-cb15-4021-9b3b-8c107b937d4c/resourceGroups/cleanupservice";
        // "0fff0b88-539a-4eda-86e4-d507bfdd8928";
        var deploymentName = Guid.NewGuid().ToString();

        // Test
        var changesNeeded = await this.Test(armClient, scope, deploymentName);
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

    private async Task<IEnumerable<WhatIfChange>> Test(ArmResourceClient armClient, string scope, string deploymentName)
    {
        var requestContent = await File.ReadAllTextAsync("deploymentWhatIfRequest.json");
        var request = JsonConvert.DeserializeObject<DeploymentWhatIf>(requestContent);

        this.logger?.LogInformation("Starting WhatIf operation...");
        var responseRaw = await armClient.PostAsync(
            $"{scope}/providers/Microsoft.Resources/deployments/{deploymentName}/whatIf",
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

    //private async Task<DeploymentExtended> Set(ArmResourceClient client, DesiredStateResource desiredState, string template, string parameters)
    //{
    //    var deployment = new Deployment
    //    {
    //        Properties = new DeploymentProperties
    //        {
    //            Template = template.ToString(),
    //            Parameters = parameters.ToString(),
    //            Mode = DeploymentMode.Incremental,
    //        },
    //    };
    //    this.logger?.LogInformation("Starting Deployment operation...");
    //    var deploymentResult = await client.Deployments.CreateOrUpdateAtScopeAsync(desiredState.Properties.Scope, desiredState.Name, deployment);
    //    this.logger?.LogInformation("Deployment operation completed. Status: {status}", deploymentResult.Properties.ProvisioningState);

    //    return deploymentResult;
    //}
}
