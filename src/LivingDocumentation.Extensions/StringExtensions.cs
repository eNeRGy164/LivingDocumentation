namespace LivingDocumentation;

public static class StringExtensions
{
    public static string ClassName(this string fullName)
    {
        if (fullName is null)
        {
            return string.Empty;
        }

        return fullName.Substring(Math.Min(fullName.LastIndexOf('.') + 1, fullName.Length));
    }

    public static string Namespace(this string fullName)
    {
        if (fullName is null)
        {
            return string.Empty;
        }

        return fullName.Substring(0, Math.Max(fullName.LastIndexOf('.'), 0));
    }

    public static IReadOnlyList<string> NamespaceParts(this string fullName)
    {
        if (fullName is null)
        {
            return new List<string>(0);
        }

        return fullName.Split('.');
    }
}
