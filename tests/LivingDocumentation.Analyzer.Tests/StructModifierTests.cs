namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class StructModifierTests
{
    [DataRow("struct Test {}", Modifier.Internal, DisplayName = "A type description about a struct without a modifier should contain the `internal` modifier")]
    [DataRow("public struct Test {}", Modifier.Public, DisplayName = "A type description about a `public` struct should contain the `public` modifier")]
    [DataRow("internal struct Test {}", Modifier.Internal, DisplayName = "A type description about an `internal` struct should contain the `internal` modifier")]
    [TestMethod]
    public void StructsShouldHaveTheCorrectAccessModifiers(string @struct, Modifier modifier)
    {
        // Assign
        var source = @struct;

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("readonly struct Test {}", Modifier.Readonly, DisplayName = "A type description about a `readonly` class should contain the `readonly` modifier")]
    [DataRow("unsafe struct Test {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` class should contain the `unsafe` modifier")]
    public void StructsShouldHaveTheCorrectModifiers(string @struct, Modifier modifier)
    {
        // Assign
        var source = @struct;

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().HaveFlag(modifier);
    }
    
    [DataRow("struct NestedTest {}", Modifier.Private, DisplayName = "A type description about a nested struct without a modifier should contain the `private` modifier")]
    [DataRow("private struct NestedTest {}", Modifier.Private, DisplayName = "A type description about a `private` nested struct should contain the `private` modifier")]
    [DataRow("public struct NestedTest {}", Modifier.Public, DisplayName = "A type description about a `public` nested struct should contain the `public` modifier")]
    [DataRow("internal struct NestedTest {}", Modifier.Internal, DisplayName = "A type description about an `internal` nested struct should contain the `internal` modifier")]
    [TestMethod]
    public void NestedStructsShouldHaveTheCorrectAccessModifiers(string @struct, Modifier modifier)
    {
        // Assign
        var source = @$"
        struct Test
        {{
            {@struct}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[1].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("partial struct NestedTest {}", Modifier.Partial, DisplayName = "A type description about a `partial` nested struct should contain the `partial` modifier")]
    [DataRow("readonly struct NestedTest {}", Modifier.Readonly, DisplayName = "A type description about a `readonly` nested struct should contain the `readonly` modifier")]
    [DataRow("unsafe struct NestedTest {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` nested class struct contain the `unsafe` modifier")]
    public void NestedStructsShouldHaveTheCorrectModifiers(string @struct, Modifier modifier)
    {
        // Assign
        var source = @$"
        struct Test
        {{
            {@struct}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[1].Modifiers.Should().HaveFlag(modifier);
    }
}
