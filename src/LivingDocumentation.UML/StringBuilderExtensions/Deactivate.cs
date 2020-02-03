using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Deactivates a life line.
        /// </summary>
        /// <param name="name">The name of the life line to deactivate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, empty of only white space.</exception>
        public static void Deactivate(this StringBuilder stringBuilder, string name)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            stringBuilder.Append(Constant.Deactivate);
            stringBuilder.Append(Constant.Space);
            stringBuilder.Append(name);
            stringBuilder.AppendNewLine();
        }
    }
}
