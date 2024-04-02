namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// The resource management error additional info.
/// </summary>
[DataContract]
public class ErrorAdditionalInfo
{
    /// <summary>
    /// The additional info type.
    /// </summary>
    /// <value>The additional info type.</value>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    /// <summary>
    /// The additional info.
    /// </summary>
    /// <value>The additional info.</value>
    [DataMember(Name = "info", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "info")]
    public object Info { get; set; }
}
