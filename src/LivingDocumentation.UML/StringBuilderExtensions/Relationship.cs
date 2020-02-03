using System;
using System.Text;

namespace LivingDocumentation.Uml
{
    public static partial class StringBuilderExtensions
    {
        /// <summary>
        /// Renders a relationship.
        /// </summary>
        /// <param name="left">The left side of the relationship.</param>
        /// <param name="type">The type of relationship.</param>
        /// <param name="right">The right side of the relationship.</param>
        /// <param name="label">Optional label for the relationship.</param>
        /// <param name="leftCardinality">Optional cardinality on the left side of the relationship.</param>
        /// <param name="rightCardinality">Optional cardinality on the right side of the relationship.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="stringBuilder"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="left"/>, <paramref name="type"/> or <paramref name="right"/> is <c>null</c>, empty of only white space.</exception>
        public static void Relationship(this StringBuilder stringBuilder, string left, string type, string right, string label = null, string leftCardinality = null, string rightCardinality = null)
        {
            if (stringBuilder is null) throw new ArgumentNullException(nameof(stringBuilder));

            if (string.IsNullOrWhiteSpace(left)) throw new ArgumentException("A non-empty value should be provided", nameof(left));
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("A non-empty value should be provided", nameof(type));
            if (string.IsNullOrWhiteSpace(right)) throw new ArgumentException("A non-empty value should be provided", nameof(right));

            stringBuilder.Append(left);
            stringBuilder.Append(Constant.Space);

            if (!string.IsNullOrEmpty(leftCardinality))
            {
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(leftCardinality);
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(type);
            stringBuilder.Append(Constant.Space);

            if (!string.IsNullOrEmpty(rightCardinality))
            {
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(rightCardinality);
                stringBuilder.Append(Constant.Quote);
                stringBuilder.Append(Constant.Space);
            }

            stringBuilder.Append(right);

            if (!string.IsNullOrEmpty(label))
            {
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(Constant.Colon);
                stringBuilder.Append(Constant.Space);
                stringBuilder.Append(label);
            }

            stringBuilder.AppendNewLine();
        }
    }
}
