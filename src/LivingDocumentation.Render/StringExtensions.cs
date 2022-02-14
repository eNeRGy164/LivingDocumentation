namespace LivingDocumentation;

public static class StringExtensions
{
    public static bool IsEnumerable(this string type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        if (!type.StartsWith("System.Collections.", StringComparison.Ordinal))
        {
            return false;
        }
        else if (type.StartsWith("System.Collections.Generic.", StringComparison.Ordinal))
        {
            return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Exception");
        }
        else if (type.StartsWith("System.Collections.Concurrent.", StringComparison.Ordinal))
        {
            return !type.Contains("Partition");
        }

        return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Structural") && !type.Contains("Provider");
    }

    public static bool IsGeneric(this string type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.IndexOf('>') > -1 && type.TrimEnd().EndsWith(">");
    }

    public static IReadOnlyList<string> GenericTypes(this string type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        if (!type.IsGeneric())
        {
            return new List<string>(0);
        }

        type = type.Trim();

        var typeParts = type.Substring(type.IndexOf('<') + 1, type.Length - type.IndexOf('<') - 2).Split(',');
        var types = new List<string>();

        foreach (var part in typeParts)
        {
            if (part.IndexOf('>') > -1 && types.Count > 0 && types.Last().ToCharArray().Count(c => c == '<') > types.Last().ToCharArray().Count(c => c == '>'))
            {
                types[types.Count - 1] = types[types.Count - 1] + "," + part.Trim();
            }
            else
            {
                types.Add(part.Trim());
            }
        }

        return types;
    }

    public static string ForDiagram(this string type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        if (type.IsGeneric())
        {
            var a = type.Substring(0, type.IndexOf('<')).ForDiagram();
            var b = type.GenericTypes().Select(s => s.ForDiagram());
            return $"{a}<{string.Join(", ", b)}>";
        }
        else if (type.IndexOf('.') > -1)
        {
            return type.Substring(type.LastIndexOf('.') + 1);
        }
        else
        {
            return type;
        }
    }

    public static string ToSentenceCase(this string type)
    {
        if (string.IsNullOrEmpty(type))
        {
            return type;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append(char.ToUpper(type[0]));

        for (var i = 1; i < type.Length; i++)
        {
            if ((char.IsUpper(type[i]) && (!char.IsUpper(type[i - 1]) || ((i + 1 < type.Length) && !char.IsUpper(type[i + 1])))) || (char.IsDigit(type[i]) && !char.IsDigit(type[i - 1])))
            {
                stringBuilder.Append(' ');
            }

            stringBuilder.Append(type[i]);
        }

        return stringBuilder.ToString();
    }
}
