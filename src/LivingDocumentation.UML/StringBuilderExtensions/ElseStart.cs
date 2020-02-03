using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders the beginning of a group.
        /// </summary>
        /// <param name="text">Optional text.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void ElseStart(this StringBuilder stringBuilder, string text = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.GroupStart(Constant.Else, text);
        }
    }
}
