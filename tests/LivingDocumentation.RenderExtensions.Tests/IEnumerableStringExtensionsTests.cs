namespace LivingDocumentation.RenderExtensions.Tests;

[TestClass]
public class IEnumerableStringExtensionsTests
{
    [TestMethod]
    public void ExtensionMethodShouldGuardAgainstNRE()
    {
        // Assign
        IEnumerable<string> list = default;

        // Act
        Action act = () => list.StartsWith("");

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("list");
    }

    [TestMethod]
    public void ExtensionMethodShouldGuardAgainstParameterNRE()
    {
        // Assign
        var list = new List<string>();

        // Act
        Action act = () => list.StartsWith(null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("partialName");
    }

    [DataRow("", 4, DisplayName = "An empty string will match against every entry")]
    [DataRow("System.Object", 1, DisplayName = "An exact match")]
    [DataRow("LivingDocumentation", 2, DisplayName = "An item matching multiple entries")]
    [DataRow("LivingDocumentation.RenderExtensions.Tests.Class<", 1, DisplayName = "Match against an entry with a generic pattern")]
    [TestMethod]
    public void ExpectTheFilterToBeAppliedCorrectlyOnTheList(string value, int expectation)
    {
        // Assign
        var list = new List<string>()
        {
            "",
            "System.Object",
            "LivingDocumentation.RenderExtensions.Tests.Class",
            "LivingDocumentation.RenderExtensions.Tests.Class<Generic>"
        };

        // Act
        var result = list.StartsWith(value);

        // Assert
        result.Should().HaveCount(expectation);
    }
}
