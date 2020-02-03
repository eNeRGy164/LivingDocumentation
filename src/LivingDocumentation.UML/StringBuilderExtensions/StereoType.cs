using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a stereotype.
        /// </summary>
        /// <param name="stereotype">Optional sterotype name.</param>
        /// <param name="customSpot">Optional custom spot.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        public static void StereoType(this StringBuilder stringBuilder, string stereotype = null, CustomSpot? customSpot = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            stringBuilder.Append(Constant.SterotypeStart);

            if (customSpot.HasValue)
            {
                stringBuilder.Append(Constant.CustomSpotStart);
                stringBuilder.Append(customSpot.Value.Character);
                stringBuilder.Append(Constant.Comma);
                stringBuilder.Append(customSpot.Value.Color);
                stringBuilder.Append(Constant.CustomSpotEnd);
            }

            stringBuilder.Append(stereotype);
            stringBuilder.Append(Constant.SterotypeEnd);
            stringBuilder.Append(Constant.Space);
        }
    }
}
