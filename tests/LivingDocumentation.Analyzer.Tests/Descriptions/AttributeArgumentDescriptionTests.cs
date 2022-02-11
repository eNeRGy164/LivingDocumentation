namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class AttributeArgumentDescriptionTests
{
    [TestMethod]
    public void AttributeArgumentDescription_Constructor_Should_SetType()
    {
        var description = new AttributeArgumentDescription("", "System.String", "");

        description.Type.Should().Be("System.String");
    }

    [TestMethod]
    public void AttributeArgumentDescription_Constructor_Should_SetName()
    {
        var description = new AttributeArgumentDescription("TestArgument", "", "");

        description.Name.Should().Be("TestArgument");
    }

    [TestMethod]
    public void AttributeArgumentDescription_Constructor_Should_SetValue()
    {
        var description = new AttributeArgumentDescription("", "", "TestValue");

        description.Value.Should().Be("TestValue");
    }
}
