namespace LivingDocumentation.RenderExtensions.Tests;

[TestClass]
public class IEnumerableIAttributeDescriptionExtensionsTests
{
    [DataRow("OfType", DisplayName = "OfType(string) should guard against a null reference exception")]
    [DataRow("HasAttribute", DisplayName = "HasAttribute(string) should guard against a null reference exception")]
    [TestMethod]
    public void ExtensionMethodShouldGuardAgainstNRE(string methodName)
    {
        // Assign
        IEnumerable<IAttributeDescription> list = default;

        var method = typeof(IEnumerableIAttributeDescriptionExtensions).GetMethod(methodName);
        var parameters = new object[] { list, "" }.ToArray();

        // Act
        Action action = () => method.Invoke(null, parameters);

        // Assert
        action.Should().ThrowExactly<System.Reflection.TargetInvocationException>()
            .WithInnerExceptionExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("list");
    }

    [DataRow("Attribute1Attribute", 1, DisplayName = "When the attribute exists in the list, return the exact match")]
    [DataRow("Attribute3Attribute", 2, DisplayName = "When the attribute exists multiple times in the list, return all matches")]
    [DataRow("Attribute4Attribute", 0, DisplayName = "When the attribute does not exists in the list, return no matches")]
    [DataRow("attribute1attribute", 0, DisplayName = "When the attribute exists with a different casing in the list, return no matches")]
    [TestMethod]
    public void ExpectTheFilterToBeAppliedCorrectlyOnTheList(string value, int expectation)
    {
        // Assign
        var list = new List<IAttributeDescription>()
        {
            new AttributeDescription("Attribute1Attribute", "Attribute1"),
            new AttributeDescription("Attribute2Attribute", "Attribute2"),
            new AttributeDescription("Attribute3Attribute", "Attribute3"),
            new AttributeDescription("Attribute3Attribute", "Attribute3")
        };

        // Act
        var result = list.OfType(value);

        // Assert
        result.Should().HaveCount(expectation);
    }

    [DataRow("Attribute1Attribute", true, DisplayName = "When the attribute exists in the list, return `true`")]
    [DataRow("Attribute3Attribute", true, DisplayName = "When the attribute exists multiple times in the list, return `true`")]
    [DataRow("Attribute4Attribute", false, DisplayName = "When the attribute does not exists in the list, return `false`")]
    [DataRow("attribute1attribute", false, DisplayName = "When the attribute exists with a different casing in the list, return `false`")]
    [TestMethod]
    public void ReturnsTheRightValueIndicatingWhetherThereIsAnAttibuteWithTheTypeInTheList(string value, bool expectation)
    {
        // Assign
        var list = new List<IAttributeDescription>()
            {
                new AttributeDescription("Attribute1Attribute", "Attribute1"),
                new AttributeDescription("Attribute2Attribute", "Attribute2"),
                new AttributeDescription("Attribute3Attribute", "Attribute3"),
                new AttributeDescription("Attribute3Attribute", "Attribute3")
            };

        // Act
        var result = list.HasAttribute(value);

        // Assert
        result.Should().Be(expectation);
    }
}
