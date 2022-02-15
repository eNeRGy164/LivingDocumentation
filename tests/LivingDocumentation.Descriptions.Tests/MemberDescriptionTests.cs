namespace LivingDocumentation.Description.Tests;

[TestClass]
public class MemberDescriptionTests
{
    [TestMethod]
    public void MembersWithSameTypeAndNameShouldBeEqual()
    {
        // Act
        var descriptionX = new PropertyDescription("Type", "Name");
        var descriptionY = new PropertyDescription("Type", "Name");

        // Assert
        descriptionX.Should().Be(descriptionY);
        descriptionX.GetHashCode().Should().Be(descriptionY.GetHashCode());
    }

    [TestMethod]
    public void MembersWithDifferentTypeShouldNotBeEqual()
    {
        // Act
        var descriptionX = new PropertyDescription("Type", "Name");
        var descriptionY = new FieldDescription("Type", "Name");

        // Assert
        descriptionX.Should().NotBe(descriptionY);
        descriptionX.GetHashCode().Should().NotBe(descriptionY.GetHashCode());
    }

    [TestMethod]
    public void MembersWithDifferentNamesShouldNotBeEqual()
    {
        // Act
        var descriptionX = new PropertyDescription("Type", "NameA");
        var descriptionY = new PropertyDescription("Type", "NameB");

        // Assert
        descriptionX.Should().NotBe(descriptionY);
        descriptionX.GetHashCode().Should().NotBe(descriptionY.GetHashCode());
    }

    [TestMethod]
    public void MembersWithDifferentTypesAndNamesShouldNotBeEqual()
    {
        // Act
        var descriptionX = new PropertyDescription("Type", "NameA");
        var descriptionY = new FieldDescription("Type", "NameB");

        // Assert
        descriptionX.Should().NotBe(descriptionY);
        descriptionX.GetHashCode().Should().NotBe(descriptionY.GetHashCode());
    }
}
