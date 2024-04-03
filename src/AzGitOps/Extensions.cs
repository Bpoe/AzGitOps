namespace Microsoft.Azure.GitOps;

public static class Extensions
{
    public static bool EqualsIgnoreCase(this string left, string right)
        => string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
}