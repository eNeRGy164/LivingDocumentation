namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class ParameterDescriptionTests
{
    [TestMethod]
    public void ParameterDescription_Constructor_Should_SetType()
    {
        var description = new ParameterDescription("System.String", null);

        description.Type.Should().Be("System.String");
    }

    [TestMethod]
    public void ParameterDescription_Constructor_Should_SetName()
    {
        var description = new ParameterDescription(null, "TestParameter");

        description.Name.Should().Be("TestParameter");
    }

    [TestMethod]
    public void ParameterDescription_HasDefaultValue_Should_BeDefaultFalse()
    {
        var description = new ParameterDescription(null, null);

        description.HasDefaultValue.Should().BeFalse();
    }
}
