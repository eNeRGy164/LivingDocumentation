namespace LivingDocumentation.Uml;

/// <summary>
/// Represents a section in a larger group.
/// </summary>
[DebuggerDisplay("AltSection {GroupType} {Label}")]
public class AltSection : InteractionFragment
{
    /// <summary>
    /// The type of group, like <c>alt</c>, <c>else</c>, etc..
    /// </summary>
    public string? GroupType { get; set; }

    /// <summary>
    /// The label of this section.
    /// </summary>
    public string? Label { get; set; }
}
