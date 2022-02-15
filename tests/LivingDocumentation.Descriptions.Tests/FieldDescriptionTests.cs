namespace LivingDocumentation.Description.Tests;

[TestClass]
public class FieldDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new FieldDescription("Type", "Name");

        // Assert
        description.MemberType.Should().Be(MemberType.Field);
        description.IsInherited.Should().BeFalse();
        description.Type.Should().Be("Type");
        description.Name.Should().Be("Name");
        description.HasInitializer.Should().BeFalse();
    }

    [DataRow(null, "Name", "type", DisplayName = "Constuctor should throw when `type` is `null`")]
    [DataRow("Type", null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string type, string name, string parameterName)
    {
        // Act
        Action act = () => new FieldDescription(type, name);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }

    [TestMethod]
    public void InitializerShouldBeSetCorrectly()
    {
        // Act
        var description = new FieldDescription("Type", "Name")
        {
            Initializer = "1"
        };

        // Assert
        description.Initializer.Should().Be("1");
        description.HasInitializer.Should().BeTrue();
    }
}
