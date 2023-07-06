namespace AzGitOps.Models;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class DesiredStateResource
{
    public string Name { get; set; }

    public string Location { get; set; }

    public DesiredStateResourceProperties Properties { get; set; }
}
