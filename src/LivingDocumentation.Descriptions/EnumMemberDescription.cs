namespace LivingDocumentation;

[DebuggerDisplay("EnumMember {Name,nq}")]
public class EnumMemberDescription(string name, string? value) : MemberDescription(name)
{
    public string? Value { get; } = value;

    public override MemberType MemberType => MemberType.EnumMember;
}
