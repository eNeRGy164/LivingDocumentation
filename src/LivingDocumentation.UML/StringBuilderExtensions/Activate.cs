using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Activates a life line.
        /// </summary>
        /// <param name="name">The name of the life line to activate.</param>
        /// <param name="color">Optional color of the activation line.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, empty of only white space.</exception>
        public static void Activate(this StringBuilder stringBuilder, string name, string color = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            stringBuilder.Append(Constant.Activate);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(name);

            if (!string.IsNullOrWhiteSpace(color))
            {
                stringBuilder.Append(Constant.Space);

                if (!color.StartsWith(Constant.ColorPrefix, StringComparison.Ordinal))
                {
                    stringBuilder.Append(Constant.ColorPrefix);
                }

                stringBuilder.Append(color.Trim());
            }

            stringBuilder.AppendNewLine();
        }
    }
}
