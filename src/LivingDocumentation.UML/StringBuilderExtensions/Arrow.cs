using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a sequence arrow.
        /// </summary>
        /// <param name="left">The left side of the arrow.</param>
        /// <param name="type">The type of relationship.</param>
        /// <param name="right">The right side of the arrow.</param>
        /// <param name="label">Optional label for the arrow.</param>
        /// <param name="color">Optional color of the label.</param>
        /// <param name="activateTarget">Whether the target is activated.</param>
        /// <param name="activationColor">Optional color for the target activation.</param>
        /// <param name="deactivateSource">Whether the source is deactivated.</param>
        /// <param name="createInstanceTarget">Whether the target instance is created.</param>
        /// <param name="destroyInstanceTarget">Whether the target instance is destroyed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="left"/>, <paramref name="type"/> or <paramref name="right"/> is <c>null</c>, empty of only white space.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> consists of less then 2 characters.</exception>
        public static void Arrow(this StringBuilder stringBuilder, string left, string type, string right, string label = null, Color color = null, bool activateTarget = false, Color activationColor = null, bool deactivateSource = false, bool createInstanceTarget = false, bool destroyInstanceTarget = false)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(left)) throw new ArgumentException("A non-empty value should be provided", nameof(left));
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("A non-empty value should be provided", nameof(type));
            if (string.IsNullOrWhiteSpace(right)) throw new ArgumentException("A non-empty value should be provided", nameof(right));

            if (type.Length < 2) throw new ArgumentException("The arrow type must be at least 2 characters long", nameof(type));

            stringBuilder.Append(left);
            stringBuilder.Append(Constant.Space);

            if (!(color is null))
            {
                stringBuilder.Append(type.Substring(0, 1));
                stringBuilder.Append(Constant.ColorStart);
                stringBuilder.Append(color);
                stringBuilder.Append(Constant.ColorEnd);
                stringBuilder.Append(type.Substring(1));
            }
            else
            {
                stringBuilder.Append(type);
            }

            stringBuilder.Append(Constant.Space);

            stringBuilder.Append(right);

            if (activateTarget)
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.TargetActivation);
            }

            if (!(activationColor is null))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(activationColor);
            }

            if (deactivateSource)
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.SourceDeactivation);
            }

            if (createInstanceTarget)
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.TargetInstanceCreation);
            }

            if (destroyInstanceTarget)
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.TargetInstanceDestruction);
            }

            if (!string.IsNullOrEmpty(label))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.Colon);
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(label.Replace("\n", "\\n"));
            }

            stringBuilder.AppendNewLine();
        }
    }
}
