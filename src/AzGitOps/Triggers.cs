namespace AzGitOps;

using Azure.Storage.Queues;
using Microsoft.Azure.GitOps;
using Microsoft.Azure.WebJobs;

[StorageAccount("EventsQueue")]
public class Triggers
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    private readonly DeploymentOperator deploymentOperator;

    public Triggers(DeploymentOperator deploymentOperator)
        => this.deploymentOperator = deploymentOperator ?? throw new ArgumentNullException(nameof(deploymentOperator));

    [FunctionName("ProcessFromQueue")]
    [Singleton("{resourceId}")]
    public async Task ProcessFromQueue(
        [QueueTrigger("events")] QueuedItem resource,
        [Queue("events")] QueueClient outputQueue)
    {
        try
        {
            await deploymentOperator.Reconcile(resource.ResourceId);
        }
        finally
        {
            await outputQueue.SendMessageAsync(new BinaryData(resource), Interval);
        }
    }
}