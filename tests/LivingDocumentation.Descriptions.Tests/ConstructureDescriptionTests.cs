namespace LivingDocumentation.Description.Tests;

[TestClass]
public class ConstructorDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new ConstructorDescription("Name");

        // Assert
        description.MemberType.Should().Be(MemberType.Constructor);
        description.Name.Should().Be("Name");
        description.Parameters.Should().BeEmpty();
        description.Statements.Should().BeEmpty();
    }

    [DataRow(null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string name, string parameterName)
    {
        // Act
        Action act = () => new ConstructorDescription(name);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
