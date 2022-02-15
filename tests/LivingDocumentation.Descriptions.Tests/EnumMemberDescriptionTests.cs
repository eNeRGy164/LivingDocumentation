namespace LivingDocumentation.Description.Tests;

[TestClass]
public class EnumMemberDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new EnumMemberDescription("Name", "Value");

        // Assert
        description.MemberType.Should().Be(MemberType.EnumMember);
        description.IsInherited.Should().BeFalse();
        description.Name.Should().Be("Name");
        description.Value.Should().Be("Value");
    }

    [DataRow(null, null, "name", DisplayName = "Constuctor should throw when `name` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string name, string value, string parameterName)
    {
        // Act
        Action act = () => new EnumMemberDescription(name, value);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
