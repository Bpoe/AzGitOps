namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Azure Key Vault reference.
/// </summary>
[DataContract]
public class KeyVaultReference
{
    /// <summary>
    /// Azure Key Vault resource id.
    /// </summary>
    /// <value>Azure Key Vault resource id.</value>
    [DataMember(Name = "id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
}
