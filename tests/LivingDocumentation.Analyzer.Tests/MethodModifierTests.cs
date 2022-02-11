namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class MethodModifierTests
{
    [DataRow("void Method() {}", Modifier.Private, DisplayName = "A method description about a method without a modifier should contain the `private` modifier")]
    [DataRow("private void Method() {}", Modifier.Private, DisplayName = "A method description about a `private` method should contain the `private` modifier")]
    [DataRow("public void Method() {}", Modifier.Public, DisplayName = "A method description about a `public` method should contain the `public` modifier")]
    [DataRow("protected void Method() {}", Modifier.Protected, DisplayName = "A method description about a `protected` method should contain the `protected` modifier")]
    [DataRow("internal void Method() {}", Modifier.Internal, DisplayName = "A method description about a `internal` method should contain the `internal` modifier")]
    [DataRow("protected internal void Method() {}", Modifier.Protected | Modifier.Internal, DisplayName = "A method description about a `protected internal` method should contain the `protected` and `internal` modifiers")]
    [DataRow("private protected void Method() {}", Modifier.Private | Modifier.Protected, DisplayName = "A method description about a `private protected` method should contain the `private` and `protected` modifiers")]
    [TestMethod]
    public void MethodsShouldHaveTheCorrectAccessModifiers(string method, Modifier modifier)
    {
        // Assign
        var source = @$"
        class Test
        {{
            {method}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().Be(modifier);
    }

    [TestMethod]
    [DataRow("public abstract void Method();", Modifier.Abstract, DisplayName = "A method description about an `abstract` method should contain the `abstract` modifier")]
    [DataRow("async void Method() {}", Modifier.Async, DisplayName = "A method description about an `async` method should contain the `async` modifier")]
    [DataRow("extern void Method();", Modifier.Extern, DisplayName = "A method description about an `extern` method should contain the `extern` modifier")]
    [DataRow("new void MethodB() {}", Modifier.New, DisplayName = "A method description about a `new` method should contain the `new` modifier")]
    [DataRow("partial void Method();", Modifier.Partial, DisplayName = "A method description about a `partial` method should contain the `partial` modifier")]
    [DataRow("protected override void MethodB() {}", Modifier.Override, DisplayName = "A method description about an `override` method should contain the `override` modifier")]
    [DataRow("sealed protected override void MethodB() {}", Modifier.Sealed, DisplayName = "A method description about a `sealed` method should contain the `sealed` modifier")]
    [DataRow("static void Method() {}", Modifier.Static, DisplayName = "A method description about a `static` method should contain the `static` modifier")]
    [DataRow("unsafe void Method() {}", Modifier.Unsafe, DisplayName = "A method description about an `unsafe` method should contain the `unsafe` modifier")]
    [DataRow("protected virtual void Method() {}", Modifier.Virtual, DisplayName = "A method description about a `virtual` method should contain the `virtual` modifier")]
    public void MethodsShouldHaveTheCorrectModifiers(string method, Modifier modifier)
    {
        // Assign
        var source = @$"
        abstract partial class Test : ClassB
        {{
            {method}
        }}
        abstract class ClassB {{
            protected virtual void MethodB() {{}}
        }}
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source, "CS0626", "CS1998");

        // Assert
        types[0].Methods[0].Modifiers.Should().HaveFlag(modifier);
    }
}
