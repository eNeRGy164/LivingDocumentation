using System.Diagnostics;

namespace roslyn_uml
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
