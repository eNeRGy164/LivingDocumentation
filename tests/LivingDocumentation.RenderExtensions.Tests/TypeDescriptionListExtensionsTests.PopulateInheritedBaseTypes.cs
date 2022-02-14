namespace LivingDocumentation.RenderExtensions.Tests;

[TestClass]
public partial class TypeDescriptionListExtensions
{
    [TestMethod]
    public void PopulateInheritedBaseTypes_NullTypes_Should_Throw()
    {
        // Assign
        var types = (List<TypeDescription>)default;

        // Act
        Action action = () => types.PopulateInheritedBaseTypes();

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .And.ParamName.Should().Be("types");
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_NoBaseTypes_Should_NotThrow()
    {
        // Assign
        var types = new[] { 
            new TypeDescription(TypeType.Class, "Test")
        };

        // Act
        Action action = () => types.PopulateInheritedBaseTypes();

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_UnknownBaseTypes_Should_NotThrow()
    {
        // Assign
        var types = new[] {
            new TypeDescription(TypeType.Class, "Test")
            {
                BaseTypes =
                {
                    "XXX"
                }
            },
        };

        // Act
        Action action = () => types.PopulateInheritedBaseTypes();

        // Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_BaseType_Should_BeCopiedToImplementingType()
    {
        // Assign
        var types = new[] {
            new TypeDescription(TypeType.Class, "Test")
            {
                BaseTypes =
                {
                    "BaseTest"
                }
            },
            new TypeDescription(TypeType.Class, "BaseTest")
            {
                BaseTypes =
                {
                    "System.Object"
                }
            }
        };

        // Act
        types.PopulateInheritedBaseTypes();

        // Assert
        types[0].BaseTypes.Should().HaveCount(2);
        types[0].BaseTypes.Should().BeEquivalentTo("BaseTest", "System.Object");
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_BaseType_Should_NotBeAltered()
    {
        // Assign
        var types = new[] {
            new TypeDescription(TypeType.Class, "Test")
            {
                BaseTypes =
                {
                    "BaseTest"
                }
            },
            new TypeDescription(TypeType.Class, "BaseTest")
            {
                BaseTypes =
                {
                    "System.Object"
                }
            }
        };

        // Act
        types.PopulateInheritedBaseTypes();

        // Assert
        types[1].BaseTypes.Should().HaveCount(1);
        types[1].BaseTypes.Should().BeEquivalentTo("System.Object");
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_BaseTypes_Should_BeCopiedToAllInheritingLevels()
    {
        // Assign
        var types = new[] {
            new TypeDescription(TypeType.Class, "Test")
            {
                BaseTypes =
                {
                    "BaseTest"
                }
            },
            new TypeDescription(TypeType.Class, "BaseTest")
            {
                BaseTypes =
                {
                    "BaserTest"
                }
            },
            new TypeDescription(TypeType.Class, "BaserTest")
            {
                BaseTypes =
                {
                    "System.Object"
                }
            }
        };

        // Act
        types.PopulateInheritedBaseTypes();

        // Assert
        types[0].BaseTypes.Should().HaveCount(3);
        types[0].BaseTypes.Should().BeEquivalentTo("BaseTest", "BaserTest", "System.Object");

        types[1].BaseTypes.Should().HaveCount(2);
        types[1].BaseTypes.Should().BeEquivalentTo("BaserTest", "System.Object");

        types[2].BaseTypes.Should().HaveCount(1);
        types[2].BaseTypes.Should().BeEquivalentTo("System.Object");
    }

    [TestMethod]
    public void PopulateInheritedBaseTypes_BaseTypes_Should_NotBeDupplicated()
    {
        // Assign
        var types = new[] {
            new TypeDescription(TypeType.Class, "Test")
            {
                BaseTypes =
                {
                    "BaseTest",
                    "System.Object"
                }
            },
            new TypeDescription(TypeType.Class, "BaseTest")
            {
                BaseTypes =
                {
                    "System.Object"
                }
            }
        };

        // Act
        types.PopulateInheritedBaseTypes();

        // Assert
        types[0].BaseTypes.Should().HaveCount(2);
        types[0].BaseTypes.Should().BeEquivalentTo("BaseTest", "System.Object");

        types[1].BaseTypes.Should().HaveCount(1);
        types[1].BaseTypes.Should().BeEquivalentTo("System.Object");
    }
}
