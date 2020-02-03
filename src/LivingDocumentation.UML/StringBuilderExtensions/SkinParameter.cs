using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a skin parameter.
        /// </summary>
        /// <param name="name">The name of the skin parameter.</param>
        /// <param name="value">The value of the skin parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> or <paramref name="value"/> is <c>null</c>, empty of only white space.</exception>
        public static void SkinParameter(this StringBuilder stringBuilder, string name, string value)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("A non-empty value should be provided", nameof(value));

            stringBuilder.Append(Constant.SkinParam);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(name.Trim());
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(value.Trim());
            stringBuilder.AppendNewLine();
        }
    }
}
