using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Argument {Type} {Text}")]
    public class ArgumentDescription
    {
        public string Type { get; }

        public string Text { get; }

        public ArgumentDescription(string type, string text)
        {
            this.Type = type;
            this.Text = text;
        }
    }
}
