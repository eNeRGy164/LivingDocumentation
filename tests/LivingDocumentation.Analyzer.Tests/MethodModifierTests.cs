namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class MethodModifierTests
{
    [TestMethod]
    public void MethodWithoutModifier_Should_HaveDefaultPrivateModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            void Method() {}
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().Be(Modifier.Private);
    }

    [TestMethod]
    public void PublicMethod_Should_HavePublicModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            public void Method() {}
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().Be(Modifier.Public);
    }

    [TestMethod]
    public void StaticMethod_Should_HaveStaticModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            static void Method() {}
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().HaveFlag(Modifier.Static);
    }

    [TestMethod]
    public void ProtectedMethod_Should_HaveProtectedModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            protected void Method() {}
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().Be(Modifier.Protected);
    }

    [TestMethod]
    public void InternalMethod_Should_HaveInternalModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            internal void Method() {}
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Modifiers.Should().Be(Modifier.Internal);
    }
}
