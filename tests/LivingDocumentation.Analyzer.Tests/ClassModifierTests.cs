namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class ClassModifierTests
{
    [TestMethod]
    public void ClassWithoutModifier_Should_HaveDefaultInternalModifier()
    {
        // Assign
        var source = @"
        class Test
        {
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(Modifier.Internal);
    }

    [TestMethod]
    public void PublicClass_Should_HavePublicModifier()
    {
        // Assign
        var source = @"
        public class Test
        {
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(Modifier.Public);
    }

    [TestMethod]
    public void StaticClass_Should_HaveStaticModifier()
    {
        // Assign
        var source = @"
        static class Test
        {
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().HaveFlag(Modifier.Static);
    }

    [TestMethod]
    public void NestedClassWithoutModifier_Should_HaveDefaultPrivateModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            class NestedTest
            {
            }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[1].Modifiers.Should().Be(Modifier.Private);
    }

    [TestMethod]
    public void NestedPublicClass_Should_HavePublicModifier()
    {
        // Assign
        var source = @"
        class Test
        {
            public class NestedTest
            {
            }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[1].Modifiers.Should().Be(Modifier.Public);
    }
}
