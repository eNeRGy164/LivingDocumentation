using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("EnumMember {Name,nq}")]
    public class EnumMemberDescription : MemberDescription
    {
        public string Value { get; }

        public override MemberType MemberType => MemberType.EnumMember;

        public EnumMemberDescription(string name, string value)
            : base(name)
        {
            this.Value = value;
        }
    }
}