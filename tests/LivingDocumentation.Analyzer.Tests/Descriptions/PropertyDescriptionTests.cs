namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class PropertyDescriptionTests
{
    [TestMethod]
    public void PropertyDescription_Constructor_Should_SetType()
    {
        var description = new PropertyDescription("System.String", "");

        description.Type.Should().Be("System.String");
    }

    [TestMethod]
    public void PropertyDescription_Constructor_Should_SetName()
    {
        var description = new PropertyDescription("", "TestProperty");

        description.Name.Should().Be("TestProperty");
    }

    [TestMethod]
    public void PropertyDescription_MemberType_Should_BeProperty()
    {
        var description = new PropertyDescription("", "");

        description.MemberType.Should().Be(MemberType.Property);
    }

    [TestMethod]
    public void PropertyDescription_HasInitializer_Should_BeDefaultFalse()
    {
        var description = new PropertyDescription("", "");

        description.HasInitializer.Should().BeFalse();
    }

    [TestMethod]
    public void PropertyDescription_HasInitializer_Should_BeTrueIfInitializerIsSet()
    {
        var description = new PropertyDescription("", "")
        {
            Initializer = "1"
        };

        description.HasInitializer.Should().BeTrue();
    }
}
