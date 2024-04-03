namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

[DataContract]
public class Deployment
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
}