using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a page header.
        /// </summary>
        /// <param name="header">The header text.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="header"/> is <c>null</c>, empty of only white space.</exception>
        public static void Header(this StringBuilder stringBuilder, string header)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(header)) throw new ArgumentException("A non-empty value should be provided", nameof(header));

            stringBuilder.Append(Constant.Header);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(header);
            stringBuilder.AppendNewLine();
        }
    }
}
