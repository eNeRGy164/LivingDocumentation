namespace LivingDocumentation;

public static class StringExtensions
{
    private const char Dot = '.';

    public static string ClassName(this string fullName)
    {
        if (fullName is null)
        {
            return string.Empty;
        }

        return fullName.Substring(Math.Min(fullName.LastIndexOf(Dot) + 1, fullName.Length));
    }

    public static string Namespace(this string fullName)
    {
        if (fullName is null)
        {
            return string.Empty;
        }

        return fullName.Substring(0, Math.Max(fullName.LastIndexOf(Dot), 0)).Trim(Dot);
    }

    public static IReadOnlyList<string> NamespaceParts(this string fullName)
    {
        if (fullName is null)
        {
            return new List<string>(0);
        }

        return fullName.Split(new[] { Dot }, StringSplitOptions.RemoveEmptyEntries);
    }
}
