namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment operation properties.
/// </summary>
[DataContract]
public class WhatIfOperationProperties
{
    /// <summary>
    /// List of resource changes predicted by What-If operation.
    /// </summary>
    /// <value>List of resource changes predicted by What-If operation.</value>
    [DataMember(Name = "changes", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "changes")]
    public List<WhatIfChange> Changes { get; set; }
}
