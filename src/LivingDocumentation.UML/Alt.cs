using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation.Uml
{
    [DebuggerDisplay("Alt")]
    public class Alt : InteractionFragment
    {
        private readonly List<AltSection> sections = new List<AltSection>();

        public IReadOnlyList<AltSection> Sections => this.sections;

        public void AddSection(AltSection section)
        {
            section.Parent = this;

            this.sections.Add(section);
        }
    }
}
