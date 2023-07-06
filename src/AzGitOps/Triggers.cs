namespace AzGitOps;

using Microsoft.Azure.GitOps;
using Microsoft.Azure.WebJobs;

[StorageAccount("EventsQueue")]
public class Triggers
{
    private readonly DeploymentOperator deploymentOperator;

    public Triggers(DeploymentOperator deploymentOperator)
        => this.deploymentOperator = deploymentOperator ?? throw new ArgumentNullException(nameof(deploymentOperator));

    [FunctionName("ProcessFromQueue")]
    [Singleton("{resourceId}")]
    public async Task ProcessFromQueue(
        [QueueTrigger("events")] QueuedItem resource)
        => await deploymentOperator.Reconcile(resource.ResourceId);
}