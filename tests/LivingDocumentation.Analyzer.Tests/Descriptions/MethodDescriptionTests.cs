namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class MethodDescriptionTests
{
    [TestMethod]
    public void MethodDescription_Constructor_Should_SetType()
    {
        var description = new MethodDescription("System.String", "");

        description.ReturnType.Should().Be("System.String");
    }

    [TestMethod]
    public void MethodDescription_Constructor_Should_SetName()
    {
        var description = new MethodDescription("", "TestMethod");

        description.Name.Should().Be("TestMethod");
    }

    [TestMethod]
    public void MethodDescription_MemberType_Should_BeMethod()
    {
        var description = new MethodDescription("", "");

        description.MemberType.Should().Be(MemberType.Method);
    }
}
