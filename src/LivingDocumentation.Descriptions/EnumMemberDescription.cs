namespace LivingDocumentation
{
    public class EnumMemberDescription : MemberDescription
    {
        public string Value { get; }

        public EnumMemberDescription(string name, string value)
            : base(MemberType.EnumMember, name)
        {
            this.Value = value;
        }
    }
}