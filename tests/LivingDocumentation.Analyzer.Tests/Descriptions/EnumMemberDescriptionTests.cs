namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class EnumMemberDescriptionTests
{
    [TestMethod]
    public void EnumMemberDescription_Constructor_Should_SetType()
    {
        var description = new EnumMemberDescription("", "TestValue");

        description.Value.Should().Be("TestValue");
    }

    [TestMethod]
    public void EnumMemberDescription_Constructor_Should_SetName()
    {
        var description = new EnumMemberDescription("TestEnumMember", "");

        description.Name.Should().Be("TestEnumMember");
    }

    [TestMethod]
    public void EnumMemberDescription_MemberType_Should_BeEnumMember()
    {
        var description = new EnumMemberDescription("", "");

        description.MemberType.Should().Be(MemberType.EnumMember);
    }
}
