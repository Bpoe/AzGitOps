namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Specifies whether template expressions are evaluated within the scope of the parent template or nested template.
/// </summary>
[DataContract]
public class ExpressionEvaluationOptions
{
    /// <summary>
    /// The scope to be used for evaluation of parameters, variables and functions in a nested template.
    /// </summary>
    /// <value>The scope to be used for evaluation of parameters, variables and functions in a nested template.</value>
    [DataMember(Name = "scope", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "scope")]
    public string Scope { get; set; }
}
