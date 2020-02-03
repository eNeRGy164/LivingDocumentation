using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders some spacing.
        /// </summary>
        /// <param name="size">Optional size of the spacing.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void Space(this StringBuilder stringBuilder, int? size = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (!size.HasValue)
            {
                stringBuilder.Append(Constant.Spacing, 3);
            }
            else
            {
                stringBuilder.Append(Constant.Spacing, 2);
                stringBuilder.Append(size.Value);
                stringBuilder.Append(Constant.Spacing, 2);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
