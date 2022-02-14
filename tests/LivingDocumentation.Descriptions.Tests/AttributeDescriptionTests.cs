namespace LivingDocumentation.Description.Tests;

[TestClass]
public class AttributeDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new AttributeDescription("Type", "Name");

        // Assert
        description.Type.Should().Be("Type");
        description.Name.Should().Be("Name");
        description.Arguments.Should().BeEmpty();
    }

    [DataRow(null, "Text", "type", DisplayName = "Constuctor should throw when `type` is `null`")]
    [DataRow("Type", null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string type, string name, string parameterName)
    {
        // Act
        Action act = () => new AttributeDescription(type, name);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
