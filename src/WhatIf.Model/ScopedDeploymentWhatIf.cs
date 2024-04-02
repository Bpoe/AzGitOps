namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment What-if operation parameters.
/// </summary>
[DataContract]
public class ScopedDeploymentWhatIf
{
    /// <summary>
    /// The location to store the deployment data.
    /// </summary>
    /// <value>The location to store the deployment data.</value>
    [DataMember(Name = "location", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "location")]
    public string Location { get; set; }

    /// <summary>
    /// Gets or Sets Properties
    /// </summary>
    [DataMember(Name = "properties", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "properties")]
    public DeploymentWhatIfProperties Properties { get; set; }
}
