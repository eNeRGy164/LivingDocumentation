namespace LivingDocumentation.Description.Tests;

[TestClass]
public class InvocationDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new InvocationDescription("Type", "Name");

        // Assert
        description.ContainingType.Should().Be("Type");
        description.Name.Should().Be("Name");
        description.Arguments.Should().BeEmpty();
    }

    [DataRow(null, "Name", "containingType", DisplayName = "Constuctor should throw when `containingType` is `null`")]
    [DataRow("Type", null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string containingType, string name, string parameterName)
    {
        // Act
        Action act = () => new InvocationDescription(containingType, name);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
