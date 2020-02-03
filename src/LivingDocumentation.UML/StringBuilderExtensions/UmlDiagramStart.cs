using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {

        /// <summary>
        /// Renders the start of an UML diagram.
        /// </summary>
        /// <param name="fileName">Optional file name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void UmlDiagramStart(this StringBuilder stringBuilder, string fileName = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(Constant.At);
            stringBuilder.Append(Constant.Start);
            stringBuilder.Append(Constant.Uml);

            if (!string.IsNullOrEmpty(fileName))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(fileName);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
