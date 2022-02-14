namespace LivingDocumentation.Description.Tests;

[TestClass]
public class AttributeArgumentDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new AttributeArgumentDescription("Name", "Type", "Value");

        // Assert
        description.Name.Should().Be("Name");
        description.Type.Should().Be("Type");
        description.Value.Should().Be("Value");
    }

    [DataRow(null, "=", "Value", "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [DataRow("Name", null, "Value", "type", DisplayName = "Constuctor should throw when `type` is `null`")]
    [DataRow("Name", "=", null, "value", DisplayName = "Constuctor should throw when `value` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string name, string type, string value, string parameterName)
    {
        // Act
        Action act = () => new AttributeArgumentDescription(name, type, value);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
