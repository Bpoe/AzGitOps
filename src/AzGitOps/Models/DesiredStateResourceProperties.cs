namespace AzGitOps.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WhatIf.Model;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class DesiredStateResourceProperties : DeploymentWhatIfProperties
{
    public string Scope { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public DesiredStateStatus? Status { get; set; }
}
