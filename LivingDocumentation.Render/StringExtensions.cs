using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivingDocumentation
{
    public static class StringExtensions
    {
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