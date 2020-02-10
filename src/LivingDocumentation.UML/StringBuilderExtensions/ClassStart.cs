using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders the beginning of a class.
        /// </summary>
        /// <param name="name">The name of the namespace. The name can't contain spaces.</param>
        /// <param name="displayName">Optional display name. The display name can contain spaces.</param>
        /// <param name="isAbstract">Indicates wheter the class is abstract. Default <c>false</c>.</param>
        /// <param name="stereotype">Optional stereo type.</param>
        /// <param name="customSpot">Optional custom spot.</param>
        /// <param name="backgroundColor">Optional background color.</param>
        /// <param name="extends">Optional class extension.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void ClassStart(this StringBuilder stringBuilder, string name, string displayName = null, bool isAbstract = false, string stereotype = null, CustomSpot? customSpot = null, Color backgroundColor = null, string extends = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            if (isAbstract)
            {
                stringBuilder.Append(Constant.Abstract);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(Constant.Class);
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

            if (!(extends is null))
            {
                stringBuilder.Append(Constant.GenericsStart);
                stringBuilder.Append(extends);
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

            stringBuilder.Append(Constant.ClassStart);
            stringBuilder.AppendNewLine();
        }
    }
}
