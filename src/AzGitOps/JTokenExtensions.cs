namespace Microsoft.Azure.GitOps;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

public static class JTokenExtensions
{
    /// <summary>
    /// Digs into a JToken for a sequence of index keys. It will return <see cref="null" /> when it encounters an index in the sequence that is not present.
    /// </summary>
    /// <param name="jToken">The <see cref="JToken" /> to dig into.</param>
    /// <param name="keys">The sequence of index keys to dig for.</param>
    /// <returns>The value at the given sequence index, or <see cref="null"/> if any key in the sequence is not present.</returns>
    public static JToken? Dig(this JToken jToken, params string[] keys)
    {
        var value = jToken;
        if (value == null)
        {
            return null;
        }

        foreach (var k in keys)
        {
            value = value[k];
            if (value == null || value.Type == JTokenType.Null)
            {
                return value;
            }
        }

        return value;
    }

    public static T? Dig<T>(this JToken jToken, params string[] keys)
    {
        var result = jToken.Dig(keys);
        return result is null ? default : result.Value<T>();
    }
}