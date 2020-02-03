using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a delay.
        /// </summary>
        /// <param name="message">Optional message for the delay.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void Delay(this StringBuilder stringBuilder, string message = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(Constant.Delay, 3);

            if (!(message is null))
            {
                stringBuilder.Append(message);
                stringBuilder.Append(Constant.Delay, 3);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
