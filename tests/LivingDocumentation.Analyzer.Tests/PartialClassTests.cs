namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class PartialClassTests
{
    [TestMethod]
    public void PartialClassesShouldBecomeASingleType()
    {
        // Assign
        var source = @"
        partial class Test
        {
            public string Property1 { get; }
        }

        partial class Test
        {
            public string Property2 { get; }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types.Should().HaveCount(1);
    }

    [TestMethod]
    public void MembersOfPartialClassesShouldBeCombined()
    {
        // Assign
        var source = @"
        partial class Test
        {
            public string Property1 { get; }
        }

        partial class Test
        {
            public string Property2 { get; }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Properties.Should().HaveCount(2);
    }
}
