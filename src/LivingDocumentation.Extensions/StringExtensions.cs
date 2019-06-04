using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingDocumentation
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
            return type.IndexOf('>') > -1 && type.EndsWith(">");
        }

        public static IReadOnlyList<string> GenericTypes(this string type)
        {
            if (type.IsGeneric())
            {
                var typeParts = type.Substring(type.IndexOf('<') + 1, type.Length - type.IndexOf('<') - 2).Split(',');
                var types = new List<string>();

                foreach (var part in typeParts)
                {
                    if (part.IndexOf('>') > -1 && types.Count > 0 && types.Last().ToCharArray().Count(c => c == '<') > types.Last().ToCharArray().Count(c => c == '>'))
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
    }
}