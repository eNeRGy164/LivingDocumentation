namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class EventDescriptionTests
{
    [TestMethod]
    public void EventDescription_Constructor_Should_SetType()
    {
        var description = new EventDescription("System.String", "");

        description.Type.Should().Be("System.String");
    }

    [TestMethod]
    public void EventDescription_Constructor_Should_SetName()
    {
        var description = new EventDescription("", "TestEvent");

        description.Name.Should().Be("TestEvent");
    }

    [TestMethod]
    public void EventDescription_MemberType_Should_BeProperty()
    {
        var description = new EventDescription("", "");

        description.MemberType.Should().Be(MemberType.Event);
    }

    [TestMethod]
    public void EventDescription_HasInitializer_Should_BeDefaultFalse()
    {
        var description = new EventDescription("", "");

        description.HasInitializer.Should().BeFalse();
    }

    [TestMethod]
    public void EventDescription_HasInitializer_Should_BeTrueIfInitializerIsSet()
    {
        var description = new EventDescription("", "")
        {
            Initializer = "1"
        };

        description.HasInitializer.Should().BeTrue();
    }
}
