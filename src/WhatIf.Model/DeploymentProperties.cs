namespace WhatIf.Model;

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Deployment properties.
/// </summary>
[DataContract]
public class DeploymentProperties
{
    /// <summary>
    /// The template content. You use this element when you want to pass the template syntax directly in the request rather than link to an existing template. It can be a JObject or well-formed JSON string. Use either the templateLink property or the template property, but not both.
    /// </summary>
    /// <value>The template content. You use this element when you want to pass the template syntax directly in the request rather than link to an existing template. It can be a JObject or well-formed JSON string. Use either the templateLink property or the template property, but not both.</value>
    [DataMember(Name = "template", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "template")]
    public object Template { get; set; }

    /// <summary>
    /// Gets or Sets TemplateLink
    /// </summary>
    [DataMember(Name = "templateLink", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "templateLink")]
    public TemplateLink TemplateLink { get; set; }

    /// <summary>
    /// Name and value pairs that define the deployment parameters for the template. You use this element when you want to provide the parameter values directly in the request rather than link to an existing parameter file. Use either the parametersLink property or the parameters property, but not both. It can be a JObject or a well formed JSON string.
    /// </summary>
    /// <value>Name and value pairs that define the deployment parameters for the template. You use this element when you want to provide the parameter values directly in the request rather than link to an existing parameter file. Use either the parametersLink property or the parameters property, but not both. It can be a JObject or a well formed JSON string.</value>
    [DataMember(Name = "parameters", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "parameters")]
    public Dictionary<string, DeploymentParameter> Parameters { get; set; }

    /// <summary>
    /// Gets or Sets ParametersLink
    /// </summary>
    [DataMember(Name = "parametersLink", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "parametersLink")]
    public ParametersLink ParametersLink { get; set; }

    /// <summary>
    /// The mode that is used to deploy resources. This value can be either Incremental or Complete. In Incremental mode, resources are deployed without deleting existing resources that are not included in the template. In Complete mode, resources are deployed and existing resources in the resource group that are not included in the template are deleted. Be careful when using Complete mode as you may unintentionally delete resources.
    /// </summary>
    /// <value>The mode that is used to deploy resources. This value can be either Incremental or Complete. In Incremental mode, resources are deployed without deleting existing resources that are not included in the template. In Complete mode, resources are deployed and existing resources in the resource group that are not included in the template are deleted. Be careful when using Complete mode as you may unintentionally delete resources.</value>
    [DataMember(Name = "mode", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "mode")]
    public string Mode { get; set; }

    /// <summary>
    /// Gets or Sets DebugSetting
    /// </summary>
    [DataMember(Name = "debugSetting", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "debugSetting")]
    public DebugSetting DebugSetting { get; set; }

    /// <summary>
    /// Gets or Sets OnErrorDeployment
    /// </summary>
    [DataMember(Name = "onErrorDeployment", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "onErrorDeployment")]
    public OnErrorDeployment OnErrorDeployment { get; set; }

    /// <summary>
    /// Gets or Sets ExpressionEvaluationOptions
    /// </summary>
    [DataMember(Name = "expressionEvaluationOptions", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "expressionEvaluationOptions")]
    public ExpressionEvaluationOptions ExpressionEvaluationOptions { get; set; }
}
