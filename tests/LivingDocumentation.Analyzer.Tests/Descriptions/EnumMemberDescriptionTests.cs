using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class EnumMemberDescriptionTests
    {
        [TestMethod]
        public void EnumMemberDescription_Constructor_Should_SetType()
        {
            var description = new EnumMemberDescription(null, "TestValue");

            description.Value.Should().Be("TestValue");
        }

        [TestMethod]
        public void EnumMemberDescription_Constructor_Should_SetName()
        {
            var description = new EnumMemberDescription("TestEnumMember", null);

            description.Name.Should().Be("TestEnumMember");
        }

        [TestMethod]
        public void EnumMemberDescription_MemberType_Should_BeEnumMember()
        {
            var description = new EnumMemberDescription(null, null);

            description.MemberType.Should().Be(MemberType.EnumMember);
        }
    }
}
