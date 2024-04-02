namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment on error behavior.
/// </summary>
[DataContract]
public class OnErrorDeployment
{
    /// <summary>
    /// The deployment on error behavior type. Possible values are LastSuccessful and SpecificDeployment.
    /// </summary>
    /// <value>The deployment on error behavior type. Possible values are LastSuccessful and SpecificDeployment.</value>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// The deployment to be used on error case.
    /// </summary>
    /// <value>The deployment to be used on error case.</value>
    [DataMember(Name = "deploymentName", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "deploymentName")]
    public string DeploymentName { get; set; }
}
