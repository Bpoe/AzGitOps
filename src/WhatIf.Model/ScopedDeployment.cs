namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment operation parameters.
/// </summary>
[DataContract]
public class ScopedDeployment
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
    public DeploymentProperties Properties { get; set; }

    /// <summary>
    /// Deployment tags
    /// </summary>
    /// <value>Deployment tags</value>
    [DataMember(Name = "tags", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "tags")]
    public Dictionary<string, string> Tags { get; set; }
}
