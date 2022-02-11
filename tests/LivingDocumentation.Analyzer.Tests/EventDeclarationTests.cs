namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class EventDeclarationTests
{
    [TestMethod]
    public void EventDeclaration_Should_CreateEventDescription()
    {
        // Assign
        var source = @"
        class Test
        {
            event System.Action @event;

            Test() { @event(); }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Events.Should().HaveCount(1);
    }

    [TestMethod]
    public void EventDeclaration_Should_CreateEventDescriptionTests2()
    {
        // Assign
        var source = @"
        class Test
        {
            event System.Action @event;

            Test() { @event(); }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Events.Should().HaveCount(1);
    }

    [TestMethod]
    public void MultipleEventDeclaration_Should_CreateEventDescriptionPerEvent()
    {
        // Assign
        var source = @"
        class Test
        {
            event System.Action event1, event2;

            Test() { event1(); event2(); }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Events.Should().HaveCount(2);
    }
}
