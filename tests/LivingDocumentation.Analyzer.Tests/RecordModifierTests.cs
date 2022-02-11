namespace LivingDocumentation.Analyzer.Tests;

#if NET6_0_OR_GREATER

[TestClass]
public class RecordModifierTests
{
    [DataRow("record Test();", Modifier.Internal, DisplayName = "A type description about a record without a modifier should contain the `internal` modifier")]
    [DataRow("public record Test();", Modifier.Public, DisplayName = "A type description about a `public` record should contain the `public` modifier")]
    [DataRow("internal record Test();", Modifier.Internal, DisplayName = "A type description about an `internal` record should contain the `internal` modifier")]
    [DataRow("record class Test();", Modifier.Internal, DisplayName = "A type description about a record class without a modifier should contain the `internal` modifier")]
    [DataRow("public record class Test();", Modifier.Public, DisplayName = "A type description about a `public` record class should contain the `public` modifier")]
    [DataRow("internal record class Test();", Modifier.Internal, DisplayName = "A type description about an `internal` record class should contain the `internal` modifier")]
    [DataRow("record struct Test();", Modifier.Internal, DisplayName = "A type description about a record struct without a modifier should contain the `internal` modifier")]
    [DataRow("public record struct Test();", Modifier.Public, DisplayName = "A type description about a `public` record struct should contain the `public` modifier")]
    [DataRow("internal record struct Test();", Modifier.Internal, DisplayName = "A type description about an `internal` record struct should contain the `internal` modifier")]
    [TestMethod]
    public void RecordsShouldHaveTheCorrectAccessModifiers(string @record, Modifier modifier)
    {
        // Assign
        var source = @record;

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("abstract record Test {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` record should contain the `abstract` modifier")]
    [DataRow("unsafe record Test {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` record should contain the `unsafe` modifier")]
    [DataRow("abstract record class Test {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` record class should contain the `abstract` modifier")]
    [DataRow("unsafe record class Test {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` record class should contain the `unsafe` modifier")]
    [DataRow("readonly record struct Test {}", Modifier.Readonly, DisplayName = "A type description about an `readonly` record struct should contain the `readonly` modifier")]
    [DataRow("unsafe record struct Test {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` record struct should contain the `unsafe` modifier")]
    public void RecordsShouldHaveTheCorrectModifiers(string @record, Modifier modifier)
    {
        // Assign
        var source = @record;

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

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
    public void NestedRecordsShouldHaveTheCorrectAccessModifiers(string @record, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test
        {{
            {@record}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[1].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("abstract record NestedTest {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` nested record should contain the `abstract` modifier")]
    [DataRow("new record NestedA {}", Modifier.New, DisplayName = "A type description about a `new` nested record should contain the `new` modifier")]
    [DataRow("partial record NestedTest {}", Modifier.Partial, DisplayName = "A type description about a `partial` nested record should contain the `partial` modifier")]
    [DataRow("unsafe record NestedTest {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` nested record should contain the `unsafe` modifier")]
    [DataRow("abstract record class NestedTest {}", Modifier.Abstract, DisplayName = "A type description about an `abstract` nested record class should contain the `abstract` modifier")]
    [DataRow("new record class NestedB {}", Modifier.New, DisplayName = "A type description about a `new` nested record class should contain the `new` modifier")]
    [DataRow("partial record class NestedTest {}", Modifier.Partial, DisplayName = "A type description about a `partial` nested record class should contain the `partial` modifier")]
    [DataRow("unsafe record class NestedTest {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` nested record class should contain the `unsafe` modifier")]
    [DataRow("new record struct NestedC {}", Modifier.New, DisplayName = "A type description about a `new` nested record struct should contain the `new` modifier")]
    [DataRow("partial record struct NestedTest {}", Modifier.Partial, DisplayName = "A type description about a `partial` nested record struct should contain the `partial` modifier")]
    [DataRow("readonly record struct NestedTest {}", Modifier.Readonly, DisplayName = "A type description about a `readonly` nested record struct should contain the `readonly` modifier")]
    [DataRow("unsafe record struct NestedTest {}", Modifier.Unsafe, DisplayName = "A type description about an `unsafe` nested record struct should contain the `unsafe` modifier")]
    public void NestedRecordsShouldHaveTheCorrectModifiers(string @record, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test : ClassB
        {{
            {@record}
        }}
        class ClassB {{
            internal record NestedA {{}};
            internal record class NestedB {{}};
            internal record struct NestedC {{}};
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0067");

        // Assert
        types[1].Modifiers.Should().HaveFlag(modifier);
    }
}

#endif
