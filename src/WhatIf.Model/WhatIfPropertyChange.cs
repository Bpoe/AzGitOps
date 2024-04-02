namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// The predicted change to the resource property.
/// </summary>
[DataContract]
public class WhatIfPropertyChange
{
    /// <summary>
    /// The path of the property.
    /// </summary>
    /// <value>The path of the property.</value>
    [DataMember(Name = "path", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "path")]
    public string Path { get; set; }

    /// <summary>
    /// The type of property change.
    /// </summary>
    /// <value>The type of property change.</value>
    [DataMember(Name = "propertyChangeType", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "propertyChangeType")]
    public string PropertyChangeType { get; set; }

    /// <summary>
    /// The value of the property before the deployment is executed.
    /// </summary>
    /// <value>The value of the property before the deployment is executed.</value>
    [DataMember(Name = "before", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "before")]
    public object Before { get; set; }

    /// <summary>
    /// The value of the property after the deployment is executed.
    /// </summary>
    /// <value>The value of the property after the deployment is executed.</value>
    [DataMember(Name = "after", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "after")]
    public object After { get; set; }

    /// <summary>
    /// Nested property changes.
    /// </summary>
    /// <value>Nested property changes.</value>
    [DataMember(Name = "children", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "children")]
    public List<WhatIfPropertyChange> Children { get; set; }
}
