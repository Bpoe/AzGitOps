namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Information about a single resource change predicted by What-If operation.
/// </summary>
[DataContract]
public class WhatIfChange
{
    /// <summary>
    /// Resource ID
    /// </summary>
    /// <value>Resource ID</value>
    [DataMember(Name = "resourceId", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "resourceId")]
    public string ResourceId { get; set; }

    /// <summary>
    /// Type of change that will be made to the resource when the deployment is executed.
    /// </summary>
    /// <value>Type of change that will be made to the resource when the deployment is executed.</value>
    [DataMember(Name = "changeType", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "changeType")]
    public string ChangeType { get; set; }

    /// <summary>
    /// The explanation about why the resource is unsupported by What-If.
    /// </summary>
    /// <value>The explanation about why the resource is unsupported by What-If.</value>
    [DataMember(Name = "unsupportedReason", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "unsupportedReason")]
    public string UnsupportedReason { get; set; }

    /// <summary>
    /// The snapshot of the resource before the deployment is executed.
    /// </summary>
    /// <value>The snapshot of the resource before the deployment is executed.</value>
    [DataMember(Name = "before", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "before")]
    public object Before { get; set; }

    /// <summary>
    /// The predicted snapshot of the resource after the deployment is executed.
    /// </summary>
    /// <value>The predicted snapshot of the resource after the deployment is executed.</value>
    [DataMember(Name = "after", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "after")]
    public object After { get; set; }

    /// <summary>
    /// The predicted changes to resource properties.
    /// </summary>
    /// <value>The predicted changes to resource properties.</value>
    [DataMember(Name = "delta", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "delta")]
    public List<WhatIfPropertyChange> Delta { get; set; }
}
