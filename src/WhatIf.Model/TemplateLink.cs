namespace WhatIf.Model;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Entity representing the reference to the template.
/// </summary>
[DataContract]
public class TemplateLink
{
    /// <summary>
    /// The URI of the template to deploy. Use either the uri or id property, but not both.
    /// </summary>
    /// <value>The URI of the template to deploy. Use either the uri or id property, but not both.</value>
    [DataMember(Name = "uri", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "uri")]
    public string Uri { get; set; }

    /// <summary>
    /// The resource id of a Template Spec. Use either the id or uri property, but not both.
    /// </summary>
    /// <value>The resource id of a Template Spec. Use either the id or uri property, but not both.</value>
    [DataMember(Name = "id", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    /// <summary>
    /// The relativePath property can be used to deploy a linked template at a location relative to the parent. If the parent template was linked with a TemplateSpec, this will reference an artifact in the TemplateSpec.  If the parent was linked with a URI, the child deployment will be a combination of the parent and relativePath URIs
    /// </summary>
    /// <value>The relativePath property can be used to deploy a linked template at a location relative to the parent. If the parent template was linked with a TemplateSpec, this will reference an artifact in the TemplateSpec.  If the parent was linked with a URI, the child deployment will be a combination of the parent and relativePath URIs</value>
    [DataMember(Name = "relativePath", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "relativePath")]
    public string RelativePath { get; set; }

    /// <summary>
    /// If included, must match the ContentVersion in the template.
    /// </summary>
    /// <value>If included, must match the ContentVersion in the template.</value>
    [DataMember(Name = "contentVersion", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "contentVersion")]
    public string ContentVersion { get; set; }

    /// <summary>
    /// The query string (for example, a SAS token) to be used with the templateLink URI.
    /// </summary>
    /// <value>The query string (for example, a SAS token) to be used with the templateLink URI.</value>
    [DataMember(Name = "queryString", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "queryString")]
    public string QueryString { get; set; }
}
