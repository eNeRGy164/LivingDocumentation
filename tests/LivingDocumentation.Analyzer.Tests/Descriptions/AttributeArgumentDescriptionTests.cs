using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class AttributeArgumentDescriptionTests
    {
        [TestMethod]
        public void AttributeArgumentDescription_Constructor_Should_SetType()
        {
            var description = new AttributeArgumentDescription(null, "System.String", null);

            description.Type.Should().Be("System.String");
        }

        [TestMethod]
        public void AttributeArgumentDescription_Constructor_Should_SetName()
        {
            var description = new AttributeArgumentDescription("TestArgument", null, null);

            description.Name.Should().Be("TestArgument");
        }

        [TestMethod]
        public void AttributeArgumentDescription_Constructor_Should_SetValue()
        {
            var description = new AttributeArgumentDescription(null, null, "TestValue");

            description.Value.Should().Be("TestValue");
        }
    }
}
