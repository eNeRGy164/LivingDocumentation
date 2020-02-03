using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a class member.
        /// </summary>
        /// <param name="name">The name of the class member.</param>
        /// <param name="isStatic">Whether the member is public; default <c>false</c>.</param>
        /// <param name="isAbstract">Whether the member is abstract; default <c>false</c>.</param>
        /// <param name="visibility">Whether the members has a specific visibility; default <c>None</c>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, empty of only white space.</exception>
        public static void ClassMember(this StringBuilder stringBuilder, string name, bool isStatic = false, bool isAbstract = false, VisibilityModifier visibility = VisibilityModifier.None)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            if (isAbstract)
            {
                stringBuilder.Append(Constant.ModifierStart);
                stringBuilder.Append(Constant.Abstract);
                stringBuilder.Append(Constant.ModifierEnd);
            }

            if (isStatic)
            {
                stringBuilder.Append(Constant.ModifierStart);
                stringBuilder.Append(Constant.Static);
                stringBuilder.Append(Constant.ModifierEnd);
            }

            switch (visibility)
            {
                case VisibilityModifier.Private:
                    stringBuilder.Append(Constant.Private);
                    break;
                case VisibilityModifier.Protected:
                    stringBuilder.Append(Constant.Protected);
                    break;
                case VisibilityModifier.PackagePrivate:
                    stringBuilder.Append(Constant.PackagePrivate);
                    break;
                case VisibilityModifier.Public:
                    stringBuilder.Append(Constant.Public);
                    break;
            }

            stringBuilder.Append(name);
            stringBuilder.AppendNewLine();
        }
    }
}
