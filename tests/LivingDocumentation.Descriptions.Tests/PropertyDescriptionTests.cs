namespace LivingDocumentation.Description.Tests;

[TestClass]
public class PropertyDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new PropertyDescription("Type", "Name");

        // Assert
        description.MemberType.Should().Be(MemberType.Property);
        description.Type.Should().Be("Type");
        description.Name.Should().Be("Name");
        description.HasInitializer.Should().BeFalse();
        description.Attributes.Should().BeEmpty();
    }

    [DataRow(null, "Name", "type", DisplayName = "Constuctor should throw when `type` is `null`")]
    [DataRow("Type", null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string type, string name, string parameterName)
    {
        // Act
        Action act = () => new PropertyDescription(type, name);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }

    [TestMethod]
    public void InitializerShouldBeSetCorrectly()
    {
        // Act
        var description = new PropertyDescription("Type", "Name")
        {
            Initializer = "1"
        };

        // Assert
        description.Initializer.Should().Be("1");
        description.HasInitializer.Should().BeTrue();
    }
}
