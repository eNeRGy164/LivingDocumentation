using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders the beginning of an enum.
        /// </summary>
        /// <param name="name">The name of the enum. The name can't contain spaces.</param>
        /// <param name="displayName">Optional display name. The display name can contain spaces.</param>
        /// <param name="stereotype">Optional stereo type.</param>
        /// <param name="customSpot">Optional custom spot.</param>
        /// <param name="backgroundColor">Optional background color.</param>
        /// <param name="generics">Optional generics.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void EnumStart(this StringBuilder stringBuilder, string name, string displayName = null, string stereotype = null, CustomSpot? customSpot = null, Color backgroundColor = null, string generics = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            stringBuilder.Append(Constant.Enum);
            stringBuilder.Append(Constant.Space);

            if (!(displayName is null))
            {
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(displayName);
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.As);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(name);

            if (!(generics is null))
            {
                stringBuilder.Append(Constant.GenericsStart);
                stringBuilder.Append(generics);
                stringBuilder.Append(Constant.GenericsEnd);
            }

            stringBuilder.Append(Constant.Space);

            if (!(stereotype is null))
            {
                stringBuilder.StereoType(stereotype, customSpot);
            }

            if (!(backgroundColor is null))
            {
                stringBuilder.Append(backgroundColor);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(Constant.EnumStart);
            stringBuilder.AppendNewLine();
        }
    }
}
