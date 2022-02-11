namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class InvocationDescriptionTests
{
    [TestMethod]
    public void InvocationDescription_Constructor_Should_SetName()
    {
        var description = new InvocationDescription("", "TestInvocation");

        description.Name.Should().Be("TestInvocation");
    }

    [TestMethod]
    public void InvocationDescription_Constructor_Should_SetContainingType()
    {
        var description = new InvocationDescription("System.String", "");

        description.ContainingType.Should().Be("System.String");
    }

    [TestMethod]
    public void InvocationDescription_Arguments_Should_BeEmpty()
    {
        var description = new InvocationDescription("", "");

        description.Arguments.Should().BeEmpty();
    }
}
