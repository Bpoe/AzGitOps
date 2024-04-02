namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Azure Key Vault parameter reference.
/// </summary>
[DataContract]
public class KeyVaultParameterReference
{
    /// <summary>
    /// Gets or Sets KeyVault
    /// </summary>
    [DataMember(Name = "keyVault", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "keyVault")]
    public KeyVaultReference KeyVault { get; set; }

    /// <summary>
    /// Azure Key Vault secret name.
    /// </summary>
    /// <value>Azure Key Vault secret name.</value>
    [DataMember(Name = "secretName", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "secretName")]
    public string SecretName { get; set; }

    /// <summary>
    /// Azure Key Vault secret version.
    /// </summary>
    /// <value>Azure Key Vault secret version.</value>
    [DataMember(Name = "secretVersion", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "secretVersion")]
    public string SecretVersion { get; set; }
}
