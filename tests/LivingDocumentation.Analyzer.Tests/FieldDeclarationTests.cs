namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class FieldDeclarationTests
{
    [TestMethod]
    public void FieldDeclaration_Should_CreateFieldDescription()
    {
        // Assign
        var source = @"
        public class Test
        {
            public string field;
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Fields.Should().HaveCount(1);
    }

    [TestMethod]
    public void MultipleFieldDeclaration_Should_CreateFieldDescriptionPerField()
    {
        // Assign
        var source = @"
        public class Test
        {
            public string field1, field2;
        }
        ";

        // Act
        var types = TestHelper.VisitSyntaxTree(source);

        // Assert
        types[0].Fields.Should().HaveCount(2);
    }
}
