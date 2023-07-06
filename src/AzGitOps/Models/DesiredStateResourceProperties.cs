namespace AzGitOps.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class DesiredStateResourceProperties
{
    public string ParametersPath { get; set; }

    public string TemplatePath { get; set; }

    public string Scope { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DesiredStateStatus? Status { get; set; }
}
