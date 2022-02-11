namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class DocumentationCommentsDescriptionTests
{
    [TestMethod]
    public void DefaultDocumentationSummary_Should_NotBeNull()
    {
        // Assign
        var documentation = new DocumentationCommentsDescription();

        // Assert
        documentation.Summary.Should().NotBeNull();
    }

    [TestMethod]
    public void DefaultDocumentationReturns_Should_NotBeNull()
    {
        // Assign
        var documentation = new DocumentationCommentsDescription();

        // Assert
        documentation.Returns.Should().NotBeNull();
    }

    [TestMethod]
    public void DefaultDocumentationRemarks_Should_NotBeNull()
    {
        // Assign
        var documentation = new DocumentationCommentsDescription();

        // Assert
        documentation.Remarks.Should().NotBeNull();
    }

    [TestMethod]
    public void DefaultDocumentationValue_Should_NotBeNull()
    {
        // Assign
        var documentation = new DocumentationCommentsDescription();

        // Assert
        documentation.Value.Should().NotBeNull();
    }

    [TestMethod]
    public void DefaultDocumentationExample_Should_NotBeNull()
    {
        // Assign
        var documentation = new DocumentationCommentsDescription();

        // Assert
        documentation.Example.Should().NotBeNull();
    }
}
