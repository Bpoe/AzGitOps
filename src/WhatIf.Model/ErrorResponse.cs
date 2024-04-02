namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Common error response for all Azure Resource Manager APIs to return error details for failed operations. (This also follows the OData error response format.)
/// </summary>
[DataContract]
public class ErrorResponse
{
    /// <summary>
    /// The error code.
    /// </summary>
    /// <value>The error code.</value>
    [DataMember(Name = "code", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "code")]
    public string Code { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    /// <value>The error message.</value>
    [DataMember(Name = "message", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// The error target.
    /// </summary>
    /// <value>The error target.</value>
    [DataMember(Name = "target", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "target")]
    public string Target { get; set; }

    /// <summary>
    /// The error details.
    /// </summary>
    /// <value>The error details.</value>
    [DataMember(Name = "details", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "details")]
    public List<ErrorResponse> Details { get; set; }

    /// <summary>
    /// The error additional info.
    /// </summary>
    /// <value>The error additional info.</value>
    [DataMember(Name = "additionalInfo", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "additionalInfo")]
    public List<ErrorAdditionalInfo> AdditionalInfo { get; set; }
}
