namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class EventDeclarationTests
{
    [DataRow("event System.Action @event;", Modifier.Private, DisplayName = "An event description about an event without a modifier should contain the `private` modifier")]
    [DataRow("private event System.Action @event;", Modifier.Private, DisplayName = "An event description about a `private` event should contain the `private` modifier")]
    [DataRow("public event System.Action @event;", Modifier.Public, DisplayName = "An event description about a `public` event should contain the `public` modifier")]
    [DataRow("protected event System.Action @event;", Modifier.Protected, DisplayName = "An event description about a `protected` event should contain the `protected` modifier")]
    [DataRow("internal event System.Action @event;", Modifier.Internal, DisplayName = "An event description about an `internal` event should contain the `internal` modifier")]
    [DataRow("protected internal event System.Action @event;", Modifier.Protected | Modifier.Internal, DisplayName = "An event description about a `protected internal` event should contain the `protected` and `internal` modifiers")]
    [DataRow("private protected event System.Action @event;", Modifier.Private | Modifier.Protected, DisplayName = "An event description about a `private protected` event should contain the `private` and `protected` modifiers")]
    [TestMethod]
    public void EventsShouldHaveTheCorrectAccessModifiers(string @event, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test
        {{
            {@event}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067");

        // Assert
        types[0].Events[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("public abstract event System.Action @event;", Modifier.Abstract, DisplayName = "An event description about an `abstract` event should contain the `abstract` modifier")]
    [DataRow("extern event System.Action @event;", Modifier.Extern, DisplayName = "An event description about an `extern` event should contain the `extern` modifier")]
    [DataRow("new event System.Action EventB;", Modifier.New, DisplayName = "An event description about a `new` event should contain the `new` modifier")]
    [DataRow("protected override event System.Action EventB;", Modifier.Override, DisplayName = "An event description about an `override` event should contain the `override` modifier")]
    [DataRow("sealed protected override event System.Action EventB;", Modifier.Sealed, DisplayName = "An event description about a `sealed` event should contain the `sealed` modifier")]
    [DataRow("static event System.Action @event;", Modifier.Static, DisplayName = "An event description about a `static` event should contain the `static` modifier")]
    [DataRow("unsafe event System.Action @event;", Modifier.Unsafe, DisplayName = "An event description about an `unsafe` event should contain the `unsafe` modifier")]
    [DataRow("protected virtual event System.Action @event;", Modifier.Virtual, DisplayName = "An event description about a `virtual` event should contain the `virtual` modifier")]
    public void EventsShouldHaveTheCorrectModifiers(string @event, Modifier modifier)
    {
        // Assign
        var source = @$"
        abstract partial class Test : ClassB
        {{
            {@event}
        }}
        abstract class ClassB {{
            protected virtual event System.Action EventB;
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067", "CS0626", "CS1998");

        // Assert
        types[0].Events[0].Modifiers.Should().HaveFlag(modifier);
    }

    [TestMethod]
    public void MultipleEventDeclarationsShouldCreateAnEventDescriptionPerEvent()
    {
        // Assign
        var source = @"
        class Test
        {
            event System.Action event1, event2;
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067");

        // Assert
        types[0].Events.Should().HaveCount(2);
    }

    [TestMethod]
    public void EventDeclaredWithAnInitialValueShouldBeRecognizedAndParsed()
    {
        // Assign
        var source = @"
        public class Test
        {
            event System.Action @event = () => {};
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);
        
        // Assert
        types[0].Events[0].HasInitializer.Should().BeTrue();
        types[0].Events[0].Initializer.Should().Be("() => {}");
    }
}
