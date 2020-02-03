using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a page footer.
        /// </summary>
        /// <param name="footer">The footer text.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="footer"/> is <c>null</c>, empty of only white space.</exception>
        public static void Footer(this StringBuilder stringBuilder, string footer)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(footer)) throw new ArgumentException("A non-empty value should be provided", nameof(footer));

            stringBuilder.Append(Constant.Footer);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(footer);
            stringBuilder.AppendNewLine();
        }
    }
}
