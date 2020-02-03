using System.Diagnostics;

namespace LivingDocumentation.Uml
{
    /// <summary>
    /// Represents an arrow between 2 participants.
    /// </summary>
    [DebuggerDisplay("{Source} -> {Target} : {Name}")]
    public class Arrow : InteractionFragment
    {
        /// <summary>
        /// The left participant of the arrow.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The right participant of the arrow.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The optional color of the arrow.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Whether the arrow is dashed.
        /// </summary>
        public bool Dashed { get; set; }

        /// <summary>
        /// The message with the arrow.
        /// </summary>
        public string Name { get; set; }
    }
}
