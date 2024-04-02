namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Entity representing the reference to the deployment parameters.
/// </summary>
[DataContract]
public class ParametersLink
{
    /// <summary>
    /// The URI of the parameters file.
    /// </summary>
    /// <value>The URI of the parameters file.</value>
    [DataMember(Name = "uri", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "uri")]
    public string Uri { get; set; }

    /// <summary>
    /// If included, must match the ContentVersion in the template.
    /// </summary>
    /// <value>If included, must match the ContentVersion in the template.</value>
    [DataMember(Name = "contentVersion", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "contentVersion")]
    public string ContentVersion { get; set; }
}
