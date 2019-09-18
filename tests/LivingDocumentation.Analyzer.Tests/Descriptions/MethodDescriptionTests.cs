using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class MethodDescriptionTests
    {
        [TestMethod]
        public void MethodDescription_Constructor_Should_SetType()
        {
            var description = new MethodDescription("System.String", null);

            description.ReturnType.Should().Be("System.String");
        }

        [TestMethod]
        public void MethodDescription_Constructor_Should_SetName()
        {
            var description = new MethodDescription(null, "TestMethod");

            description.Name.Should().Be("TestMethod");
        }

        [TestMethod]
        public void MethodDescription_MemberType_Should_BeMethod()
        {
            var description = new MethodDescription(null, null);

            description.MemberType.Should().Be(MemberType.Method);
        }
    }
}
