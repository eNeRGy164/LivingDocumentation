namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class EnumModifierTests
{
    [TestMethod]
    public void EnumWithoutModifier_Should_HaveDefaultInternalModifier()
    {
        // Assign
        var source = @"
        enum Test
        {
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(Modifier.Internal);
    }

    [TestMethod]
    public void PublicEnum_Should_HavePublicModifier()
    {
        // Assign
        var source = @"
        public enum Test
        {
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Modifiers.Should().Be(Modifier.Public);
    }

    [TestMethod]
    public void EnumMembers_Should_HavePublicModifier()
    {
        // Assign
        var source = @"
        public enum Test
        {
            Value
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].EnumMembers[0].Modifiers.Should().Be(Modifier.Public);
    }
}
