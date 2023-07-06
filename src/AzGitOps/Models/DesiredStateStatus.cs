namespace AzGitOps.Models;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class DesiredStateStatus
{
    public string Status { get; set; }

    public string Message { get; set; }

    public DateTime AssessmentStartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AssessmentEndedAt { get; set; }
}