namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class ClassModifierTests
{
    [DataRow("class Test {}", Modifier.Internal, DisplayName = "A type description about a class without a modifier should contain the `internal` modifier")]
    [DataRow("public class Test {}", Modifier.Public, DisplayName = "A type description about a `public` class should contain the `public` modifier")]
    [DataRow("internal class Test {}", Modifier.Internal, DisplayName = "A type description about an `internal` class should contain the `internal` modifier")]
    [TestMethod]
    public void ClassesShouldHaveTheCorrectAccessModifiers(string @class, Modifier modifier)
    {
        // Assign
        var source = @class;

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("public abstract class Test {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` class should contain the `abstract` modifier")]
    [DataRow("static class Test {}", Modifier.Static, DisplayName = "A type description about a `static` class should contain the `static` modifier")]
    [DataRow("unsafe class Test {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` class should contain the `unsafe` modifier")]
    public void ClassesShouldHaveTheCorrectModifiers(string @class, Modifier modifier)
    {
        // Assign
        var source = @class;

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067", "CS0626", "CS1998");

        // Assert
        types[0].Modifiers.Should().HaveFlag(modifier);
    }

    [DataRow("class NestedTest {}", Modifier.Private, DisplayName = "A type description about a nested class without a modifier should contain the `private` modifier")]
    [DataRow("private class NestedTest {}", Modifier.Private, DisplayName = "A type description about a `private` nested class should contain the `private` modifier")]
    [DataRow("public class NestedTest {}", Modifier.Public, DisplayName = "A type description about a `public` nested class should contain the `public` modifier")]
    [DataRow("protected class NestedTest {}", Modifier.Protected, DisplayName = "A type description about a `protected` nested class should contain the `protected` modifier")]
    [DataRow("internal class NestedTest {}", Modifier.Internal, DisplayName = "A type description about an `internal` nested class should contain the `internal` modifier")]
    [DataRow("protected internal class NestedTest {}", Modifier.Protected | Modifier.Internal, DisplayName = "A type description about a `protected internal` nested class should contain the `protected` and `internal` modifiers")]
    [DataRow("private protected class NestedTest {}", Modifier.Private | Modifier.Protected, DisplayName = "A type description about a `private protected` nested class should contain the `private` and `protected` modifiers")]
    [TestMethod]
    public void NestedClassesShouldHaveTheCorrectAccessModifiers(string @class, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test
        {{
            {@class}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067");

        // Assert
        types[1].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("public abstract class NestedTest {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` nested class should contain the `abstract` modifier")]
    [DataRow("new class NestedB {}", Modifier.New, DisplayName = "A type description about a `new` nested class should contain the `new` modifier")]
    [DataRow("partial class NestedTest {}", Modifier.Partial, DisplayName = "A type description about a `partial` nested class should contain the `partial` modifier")]
    [DataRow("static class NestedTest {}", Modifier.Static, DisplayName = "A type description about a `static` nested class should contain the `static` modifier")]
    [DataRow("unsafe class NestedTest {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` nested class should contain the `unsafe` modifier")]
    public void NestedClassesShouldHaveTheCorrectModifiers(string @class, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test : ClassB
        {{
            {@class}
        }}
        class ClassB {{
            internal class NestedB {{}};
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067");

        // Assert
        types[1].Modifiers.Should().HaveFlag(modifier);
    }
}
