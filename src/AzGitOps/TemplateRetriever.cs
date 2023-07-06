namespace Microsoft.Azure.GitOps;

using Newtonsoft.Json.Linq;

public class TemplateRetriever
{
    public async Task<JObject?> GetTemplate(string template)
    {
        using var stream = File.OpenRead("templates/" + template);
        if (stream == null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        return JObject.Parse(content);
    }
}