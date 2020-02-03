using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders the beginning of a group.
        /// </summary>
        /// <param name="type">The type of group.</param>
        /// <param name="text">Optional text.</param>
        /// <param name="label">Optional label.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void GroupStart(this StringBuilder stringBuilder, string type = Constant.Group, string text = null, string label = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(type);

            if (!string.IsNullOrWhiteSpace(label))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(label);
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                stringBuilder.Append(Constant.Space);
                
                if (type == Constant.Group) stringBuilder.Append(Constant.GroupLabelStart);

                stringBuilder.Append(text);

                if (type == Constant.Group) stringBuilder.Append(Constant.GroupLabelEnd);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
