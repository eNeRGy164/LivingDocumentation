namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class ConstructorDescriptionTests
{
    [TestMethod]
    public void ConstructorDescription_Constructor_Should_SetName()
    {
        var description = new ConstructorDescription("TestConstructor");

        description.Name.Should().Be("TestConstructor");
    }

    [TestMethod]
    public void ConstructorDescription_MemberType_Should_BeConstructor()
    {
        var description = new ConstructorDescription("");

        description.MemberType.Should().Be(MemberType.Constructor);
    }

    [TestMethod]
    public void ConstructorDescription_Parameters_Should_BeEmpty()
    {
        var description = new ConstructorDescription("");

        description.Parameters.Should().BeEmpty();
    }
}
