using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class EventDescriptionTests
    {
        [TestMethod]
        public void EventDescription_Constructor_Should_SetType()
        {
            var description = new EventDescription("System.String", null);

            description.Type.Should().Be("System.String");
        }

        [TestMethod]
        public void EventDescription_Constructor_Should_SetName()
        {
            var description = new EventDescription(null, "TestEvent");

            description.Name.Should().Be("TestEvent");
        }

        [TestMethod]
        public void EventDescription_MemberType_Should_BeProperty()
        {
            var description = new EventDescription(null, null);

            description.MemberType.Should().Be(MemberType.Event);
        }

        [TestMethod]
        public void EventDescription_HasInitializer_Should_BeDefaultFalse()
        {
            var description = new EventDescription(null, null);

            description.HasInitializer.Should().BeFalse();
        }

        [TestMethod]
        public void EventDescription_HasInitializer_Should_BeTrueIfInitializerIsSet()
        {
            var description = new EventDescription(null, null);

            description.Initializer = "1";

            description.HasInitializer.Should().BeTrue();
        }
    }
}
