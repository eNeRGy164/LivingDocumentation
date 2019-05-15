using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace roslyn_uml
{
    public static class StringExtensions
    {
        public static bool IsEnumerable(this string type)
        {
            if (type.StartsWith("System.Collections.Generic.", StringComparison.Ordinal))
            {
                return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Exception");
            }
            else if (type.StartsWith("System.Collections.Concurrent.", StringComparison.Ordinal))
            {
                return !type.Contains("Partition");
            }
            else if (type.StartsWith("System.Collections.", StringComparison.Ordinal))
            {
                return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Structural") && !type.Contains("Provider");
            }
            else
            {
                return false;
            }
        }

        public static bool IsGeneric(this string type)
        {
            return type.IndexOf('>') > -1 && type.Last() == '>';
        }

        public static IReadOnlyList<string> GenericTypes(this string type)
        {
            if (type.IsGeneric())
            {
                var typeParts = type.Substring(type.IndexOf('<') + 1, type.Length - type.IndexOf('<') - 2).Split(',');
                var types = new List<string>();

                foreach (var part in typeParts)
                {
                    if (part.IndexOf('>') > -1)
                    {
                        types[types.Count - 1] = types[types.Count - 1] + "," + part;
                    } else
                    {
                        types.Add(part);
                    }
                }

                return types;
            }
            else
            {
                return new List<string>(0);
            }
        }

        public static string ForDiagram(this string type)
        {
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

        public static string ToSentenceCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(char.ToUpper(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) || char.IsDigit(input[i]))
                {
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(input[i]);
            }

            return stringBuilder.ToString();
        }
    }
}