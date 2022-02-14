namespace LivingDocumentation.Description.Tests;

[TestClass]
public class MethodDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new MethodDescription("Type", "Name");

        // Assert
        description.MemberType.Should().Be(MemberType.Method);
        description.ReturnType.Should().Be("Type");
        description.Name.Should().Be("Name");
        description.Parameters.Should().BeEmpty();
        description.Statements.Should().BeEmpty();
    }

    [TestMethod]
    public void ReturnTypeShouldBeVoidWhenNull()
    {
        // Act
        var description = new MethodDescription(null, "Name");

        // Assert
        description.ReturnType.Should().Be("void");
    }
}
