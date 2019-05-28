using System;
using System.Collections.Generic;

namespace roslyn_uml.Uml
{
    public abstract class InteractionFragment
    {
        private readonly List<InteractionFragment> interactionFragments = new List<InteractionFragment>();

        public InteractionFragment Parent { get; set; }
        public virtual IReadOnlyList<InteractionFragment> Fragments => interactionFragments;

        public void AddFragment(InteractionFragment fragment)
        {
            fragment.Parent = this;

            this.interactionFragments.Add(fragment);
        }

        public void AddFragments(IEnumerable<InteractionFragment> fragments)
        {
            foreach (var fragment in fragments)
            {
                this.AddFragment(fragment);
            }
        }
    }
}
