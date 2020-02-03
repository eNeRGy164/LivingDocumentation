using System.Collections.Generic;

namespace LivingDocumentation.Uml
{
    /// <summary>
    /// Represents interaction fragments in a sequence diagram.
    /// </summary>
    public abstract class InteractionFragment
    {
        private readonly List<InteractionFragment> interactionFragments = new List<InteractionFragment>();

        /// <summary>
        /// The parent of this fragment.
        /// </summary>
        public InteractionFragment Parent { get; set; }

        /// <summary>
        /// The children of this fragment.
        /// </summary>
        public virtual IReadOnlyList<InteractionFragment> Fragments => this.interactionFragments;

        /// <summary>
        /// Add a fragment to this level.
        /// </summary>
        /// <param name="fragment">The fragment to add.</param>
        public void AddFragment(InteractionFragment fragment)
        {
            fragment.Parent = this;

            this.interactionFragments.Add(fragment);
        }

        /// <summary>
        /// Add a list of fragments to this level.
        /// </summary>
        /// <param name="fragments">The fragments to add.</param>
        public void AddFragments(IEnumerable<InteractionFragment> fragments)
        {
            foreach (var fragment in fragments)
            {
                this.AddFragment(fragment);
            }
        }
    }
}
