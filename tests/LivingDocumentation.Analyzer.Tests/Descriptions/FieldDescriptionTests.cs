namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class FieldDescriptionTests
{
    [TestMethod]
    public void FieldDescription_Constructor_Should_SetType()
    {
        var description = new FieldDescription("System.String", "");

        description.Type.Should().Be("System.String");
    }

    [TestMethod]
    public void FieldDescription_Constructor_Should_SetName()
    {
        var description = new FieldDescription("", "TestField");

        description.Name.Should().Be("TestField");
    }

    [TestMethod]
    public void FieldDescription_MemberType_Should_BeField()
    {
        var description = new FieldDescription("", "");

        description.MemberType.Should().Be(MemberType.Field);
    }

    [TestMethod]
    public void FieldDescription_HasInitializer_Should_BeDefaultFalse()
    {
        var description = new FieldDescription("", "");

        description.HasInitializer.Should().BeFalse();
    }

    [TestMethod]
    public void FieldDescription_HasInitializer_Should_BeTrueIfInitializerIsSet()
    {
        var description = new FieldDescription("", "")
        {
            Initializer = "1"
        };

        description.HasInitializer.Should().BeTrue();
    }
}
