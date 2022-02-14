namespace LivingDocumentation.Description.Tests;

[TestClass]
public class ArgumentDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new ArgumentDescription("Type", "Text");

        // Assert
        description.Type.Should().Be("Type");
        description.Text.Should().Be("Text");
    }

    [DataRow(null, "Text", "type", DisplayName = "Constuctor should throw when `type` is `null`")]
    [DataRow("Type", null, "text", DisplayName = "Constuctor should throw when `text` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string type, string text, string parameterName)
    {
        // Act
        Action act = () => new ArgumentDescription(type, text);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
