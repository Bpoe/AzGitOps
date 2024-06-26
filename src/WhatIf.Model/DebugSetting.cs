namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// The debug setting.
/// </summary>
[DataContract]
public class DebugSetting
{
    /// <summary>
    /// Specifies the type of information to log for debugging. The permitted values are none, requestContent, responseContent, or both requestContent and responseContent separated by a comma. The default is none. When setting this value, carefully consider the type of information you are passing in during deployment. By logging information about the request or response, you could potentially expose sensitive data that is retrieved through the deployment operations.
    /// </summary>
    /// <value>Specifies the type of information to log for debugging. The permitted values are none, requestContent, responseContent, or both requestContent and responseContent separated by a comma. The default is none. When setting this value, carefully consider the type of information you are passing in during deployment. By logging information about the request or response, you could potentially expose sensitive data that is retrieved through the deployment operations.</value>
    [DataMember(Name = "detailLevel", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "detailLevel")]
    public string DetailLevel { get; set; }
}
