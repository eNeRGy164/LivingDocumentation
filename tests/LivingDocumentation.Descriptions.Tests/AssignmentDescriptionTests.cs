namespace LivingDocumentation.Description.Tests;

[TestClass]
public class AssignmentDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new AssignmentDescription("Left", "=", "Right");

        // Assert
        description.Left.Should().Be("Left");
        description.Operator.Should().Be("=");
        description.Right.Should().Be("Right");
    }

    [DataRow(null, "=", "Right", "left", DisplayName = "Constuctor should throw when `left` is `null`")]
    [DataRow("Left", null, "Right", "operator", DisplayName = "Constuctor should throw when `operator` is `null`")]
    [DataRow("Left", "=", null, "right", DisplayName = "Constuctor should throw when `right` is `null`")]
    [TestMethod]
    public void ConstructorShouldGuardAgainstNullParamters(string left, string @operator, string right, string parameterName)
    {
        // Act
        Action act = () => new AssignmentDescription(left, @operator, right);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName(parameterName);
    }
}
