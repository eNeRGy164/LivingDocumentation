namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class ForEachTests
{
    [TestMethod]
    public void ForEach_Should_BeDetected()
    {
        // Assign
        var source = @"
        class Test
        {
            void Method()
            {
                var items = new string[0];
                foreach (var item in items)
                {
                }
            }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Methods[0].Statements[0].Should().BeOfType<ForEach>();
    }

    [TestMethod]
    public void ForEachStatements_Should_BeParsed()
    {
        // Assign
        var source = @"
        class Test
        {
            void Method()
            {
                var items = new string[0];
                foreach (var item in items)
                {
                    var o = new System.Object();
                }
            }
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        var forEach = types[0].Methods[0].Statements[0];
        forEach.Statements.Should().HaveCount(1);
    }
}
