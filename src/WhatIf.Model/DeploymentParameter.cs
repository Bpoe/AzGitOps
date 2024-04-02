namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment parameter for the template.
/// </summary>
[DataContract]
public class DeploymentParameter
{
    /// <summary>
    /// Input value to the parameter .
    /// </summary>
    /// <value>Input value to the parameter .</value>
    [DataMember(Name = "value", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "value")]
    public object Value { get; set; }

    /// <summary>
    /// Gets or Sets Reference
    /// </summary>
    [DataMember(Name = "reference", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "reference")]
    public KeyVaultParameterReference Reference { get; set; }
}
