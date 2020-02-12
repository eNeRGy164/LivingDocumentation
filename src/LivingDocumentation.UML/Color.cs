namespace LivingDocumentation.Uml
{
    public class Color
    {
        private readonly string value;

        public Color(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                this.value = string.Empty;
            }
            else
            {
                if (color[0] == Constant.ColorPrefix)
                {
                    this.value = color.Trim();
                }
                else
                {
                    this.value = Constant.ColorPrefix + color.Trim();
                }
            }
        }

        public Color(NamedColor color)
            : this(color.ToString("G"))
        {
        }

        public override string ToString()
        {
            return this.value;
        }

        public static implicit operator Color(NamedColor color)
        {
            return new Color(color);
        }

        public static implicit operator Color(string color)
        {
            return new Color(color);
        }
    }
}
