namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment What-If operation settings.
/// </summary>
[DataContract]
public class DeploymentWhatIfSettings
{
    /// <summary>
    /// The format of the What-If results
    /// </summary>
    /// <value>The format of the What-If results</value>
    [DataMember(Name = "resultFormat", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "resultFormat")]
    public string ResultFormat { get; set; }
}
