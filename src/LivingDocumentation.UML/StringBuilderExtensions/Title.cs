using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a page title.
        /// </summary>
        /// <param name="title">The page title.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is <c>null</c>, empty of only white space.</exception>
        public static void Title(this StringBuilder stringBuilder, string title)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("A non-empty value should be provided", nameof(title));

            stringBuilder.Append(Constant.Title);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(title);
            stringBuilder.AppendNewLine();
        }
    }
}
