using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class ArgumentDescriptionTests
    {
        [TestMethod]
        public void ArgumentDescription_Constructor_Should_SetType()
        {
            var description = new ArgumentDescription("System.String", null);

            description.Type.Should().Be("System.String");
        }

        [TestMethod]
        public void ArgumentDescription_Constructor_Should_SetName()
        {
            var description = new ArgumentDescription(null, "TestText");

            description.Text.Should().Be("TestText");
        }
    }
}
