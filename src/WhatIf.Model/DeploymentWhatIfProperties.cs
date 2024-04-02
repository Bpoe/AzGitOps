namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment What-if properties.
/// </summary>
[DataContract]
public class DeploymentWhatIfProperties : DeploymentProperties
{
    /// <summary>
    /// Gets or Sets WhatIfSettings
    /// </summary>
    [DataMember(Name = "whatIfSettings", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "whatIfSettings")]
    public DeploymentWhatIfSettings WhatIfSettings { get; set; }
}
