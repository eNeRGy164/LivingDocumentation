using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a divider.
        /// </summary>
        /// <param name="message">Optional title for the divider.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void Divider(this StringBuilder stringBuilder, string title = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(Constant.Divider, 2);

            if (!(title is null))
            {
                stringBuilder.Append(title);
            }

            stringBuilder.Append(Constant.Divider, 2);
            stringBuilder.AppendNewLine();
        }
    }
}
