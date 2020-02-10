using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a participant.
        /// </summary>
        /// <param name="name">The name of the participant.</param>
        /// <param name="displayName">Optional display name of the participant.</param>
        /// <param name="type">Optional type of the participant.</param>
        /// <param name="color">Optional color of the participant.</param>
        /// <param name="order">Optional order of the participant.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, empty of only white space.</exception>
        public static void Participant(this StringBuilder stringBuilder, string name, string displayName = null, ParticipantType type = ParticipantType.Participant, Color color = null, int? order = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A non-empty value should be provided", nameof(name));

            stringBuilder.Append(type.ToString().ToLowerInvariant());
            stringBuilder.Append(Constant.Space);

            if (!string.IsNullOrEmpty(displayName))
            {
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(displayName);
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.As);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(name);

            if (order.HasValue)
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.Order);
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(order.Value);
            }

            if (!(color is null))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(color);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
