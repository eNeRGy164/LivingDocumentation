using System.Text;

namespace roslyn_uml.eShopOnContainers
{
    public static class AsciiDocExtensions
    {
        public static string ToSentenceCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(char.ToUpper(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) || char.IsDigit(input[i]))
                {
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(input[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
