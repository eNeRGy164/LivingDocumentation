using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a box for participants.
        /// </summary>
        /// <param name="title">Optional title of the box.</param>
        /// <param name="color">Optional background color of the box.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void BoxStart(this StringBuilder stringBuilder, string title = null, string color = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(Constant.Box);

            if (!string.IsNullOrEmpty(title))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(title.Trim());
                stringBuilder.Append(Constant.Quote);
            }

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
