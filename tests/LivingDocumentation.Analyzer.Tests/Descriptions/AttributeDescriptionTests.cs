using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class AttributeDescriptionTests
    {
        [TestMethod]
        public void AttributeDescription_Constructor_Should_SetType()
        {
            var description = new AttributeDescription("System.String", null);

            description.Type.Should().Be("System.String");
        }

        [TestMethod]
        public void AttributeDescription_Constructor_Should_SetName()
        {
            var description = new AttributeDescription(null, "TestParameter");

            description.Name.Should().Be("TestParameter");
        }

        [TestMethod]
        public void AttributeDescription_Arguments_Should_BeEmpty()
        {
            var description = new AttributeDescription(null, null);

            description.Arguments.Should().BeEmpty();
        }
    }
}
