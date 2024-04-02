namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Result of the What-If operation. Contains a list of predicted changes and a URL link to get to the next set of results.
/// </summary>
[DataContract]
public class WhatIfOperationResult
{
    /// <summary>
    /// Status of the What-If operation.
    /// </summary>
    /// <value>Status of the What-If operation.</value>
    [DataMember(Name = "status", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets Properties
    /// </summary>
    [DataMember(Name = "properties", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "properties")]
    public WhatIfOperationProperties Properties { get; set; }

    /// <summary>
    /// Gets or Sets Error
    /// </summary>
    [DataMember(Name = "error", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "error")]
    public ErrorResponse Error { get; set; }
}
