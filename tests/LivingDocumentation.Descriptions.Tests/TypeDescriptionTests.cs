namespace LivingDocumentation.Description.Tests;

[TestClass]
public class TypeDescriptionTests
{
    [TestMethod]
    public void ConstructorShouldSetTypesCorrectly()
    {
        // Act
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");

        // Assert
        description.Type.Should().Be(TypeType.Class);
        description.FullName.Should().Be("Namespace.Class");
        description.Namespace.Should().Be("Namespace");
        description.Name.Should().Be("Class");
        description.BaseTypes.Should().BeEmpty();
        description.Attributes.Should().BeEmpty();
    }

    [DataRow("IsClass", TypeType.Class, true, DisplayName = "On a type description of `Class`, `IsClass()` should return `true`")]
    [DataRow("IsClass", TypeType.Enum, false, DisplayName = "On a type description of `Enum`,`IsClass()` should return `false`")]
    [DataRow("IsEnum", TypeType.Enum, true, DisplayName = "On a type description of `Enum`,`IsEnum()` should return `true`")]
    [DataRow("IsEnum", TypeType.Interface, false, DisplayName = "On a type description of `Interface`,`IsEnum()` should return `false`")]
    [DataRow("IsInterface", TypeType.Interface, true, DisplayName = "On a type description of `Interface`,`IsInterface()` should return `true`")]
    [DataRow("IsInterface", TypeType.Struct, false, DisplayName = "On a type description of `Struct`,`IsInterface()` should return `false`")]
    [DataRow("IsStruct", TypeType.Struct, true, DisplayName = "On a type description of `Struct`,`IsStruct()` should return `true`")]
    [DataRow("IsStruct", TypeType.Class, false, DisplayName = "On a type description of `Class`,`IsStruct()` should return `false`")]
    [TestMethod]
    public void TypeMethodsShouldReturnCorrectValue(string methodName, TypeType type, bool expected)
    {
        // Arrange
        var description = new TypeDescription(type, "Namespace.Class");
        var method = typeof(TypeDescription).GetMethod(methodName);

        // Act
        var result = (bool)method.Invoke(description, null);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    public void HasPropertyShouldReturnCorrectValue()
    {
        // Arrange
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");
        description.AddMember(new PropertyDescription("Type", "Name"));

        // Assert
        description.HasProperty("Name").Should().BeTrue();
        description.HasProperty("Name2").Should().BeFalse("Because property is not added");
        description.HasProperty("name").Should().BeFalse("Because name is case sensitive");
    }

    [TestMethod]
    public void HasFieldShouldReturnCorrectValue()
    {
        // Arrange
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");
        description.AddMember(new FieldDescription("Type", "Name"));

        // Assert
        description.HasField("Name").Should().BeTrue();
        description.HasField("Name2").Should().BeFalse("Because field is not added");
        description.HasField("name").Should().BeFalse("Because name is case sensitive");
    }

    [TestMethod]
    public void HasMethodShouldReturnCorrectValue()
    {
        // Arrange
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");
        description.AddMember(new MethodDescription("Type", "Name"));

        // Assert
        description.HasMethod("Name").Should().BeTrue();
        description.HasMethod("Name2").Should().BeFalse("Because method is not added");
        description.HasMethod("name").Should().BeFalse("Because name is case sensitive");
    }

    [TestMethod]
    public void HasEventShouldReturnCorrectValue()
    {
        // Arrange
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");
        description.AddMember(new EventDescription("Type", "Name"));

        // Assert
        description.HasEvent("Name").Should().BeTrue();
        description.HasEvent("Name2").Should().BeFalse("Because event is not added");
        description.HasEvent("name").Should().BeFalse("Because name is case sensitive");
    }

    [TestMethod]
    public void HasEnumMemberShouldReturnCorrectValue()
    {
        // Arrange
        var description = new TypeDescription(TypeType.Class, "Namespace.Class");
        description.AddMember(new EnumMemberDescription("Name", "0"));

        // Assert
        description.HasEnumMember("Name").Should().BeTrue();
        description.HasEnumMember("Name2").Should().BeFalse("Because enum member is not added");
        description.HasEnumMember("name").Should().BeFalse("Because name is case sensitive");
    }

    [TestMethod]
    public void TypeDescription_GetHashCode_Should_BeTheSame()
    {
        var descriptionX = new TypeDescription(0, "TestNamespace.TestClass").GetHashCode();
        var descriptionY = new TypeDescription(0, "TestNamespace.TestClass").GetHashCode();

        descriptionX.Should().Be(descriptionY);
    }

    [TestMethod]
    public void TypeDescription_GetHashCode_Should_ShouldBeTheSameForDifferentTypeTypes()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass").GetHashCode();
        var descriptionY = new TypeDescription(TypeType.Interface, "TestNamespace.TestClass").GetHashCode();

        descriptionX.Should().Be(descriptionY);
    }

    [TestMethod]
    public void TypeDescription_GetHashCode_Should_DifferentNamesShouldDiffer()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass1").GetHashCode();
        var descriptionY = new TypeDescription(TypeType.Class, "TestNamespace.TestClass2").GetHashCode();

        descriptionX.Should().NotBe(descriptionY);
    }

    [TestMethod]
    public void TypeDescription_Equals_Should_BeTrueForSameTypeDescriptions()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass");
        var descriptionY = new TypeDescription(TypeType.Class, "TestNamespace.TestClass");

        descriptionX.Equals(descriptionY).Should().BeTrue();
    }

    [TestMethod]
    public void TypeDescription_Equals_Should_BeFalseForDifferentCasing()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass");
        var descriptionY = new TypeDescription(TypeType.Class, "testnamespace.testclass");

        descriptionX.Equals(descriptionY).Should().BeFalse();
    }

    [TestMethod]
    public void TypeDescription_Equals_Should_BeFalseForDifferentTypeDescriptions()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass1");
        var descriptionY = new TypeDescription(TypeType.Class, "TestNamespace.TestClass2");

        descriptionX.Equals(descriptionY).Should().BeFalse();
    }

    [TestMethod]
    public void TypeDescription_Equals_Should_BeFalseForDifferentObjects()
    {
        var descriptionX = new TypeDescription(TypeType.Class, "TestNamespace.TestClass");
        var descriptionY = new object();

        descriptionX.Equals(descriptionY).Should().BeFalse();
    }

    [TestMethod]
    public void NamespaceShouldBeEmptyIfThereIsOnlyAClassName()
    {
        var description = new TypeDescription(TypeType.Class, "TestClass");

        description.Namespace.Should().BeEmpty();
        description.Name.Should().Be("TestClass");
    }

    [TestMethod]
    public void TypeDescription_Constructor_Should_SetFullNameEmptyIfNull()
    {
        var description = new TypeDescription(TypeType.Class, default);

        description.FullName.Should().BeEmpty();
    }

    [TestMethod]
    public void TypeDescription_AddMember_Should_ThrowNotSupportedException()
    {
        var description = new TypeDescription(default, default);

        Action action = () => { description.AddMember(new UnsupportedMemberDescription("")); };

        action.Should().Throw<NotSupportedException>();
    }

    [TestMethod]
    public void TypeDescription_MethodBodies_ConstuctorsAndMethodBodies_Should_BeReturnConcatenated()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");
        var c = new ConstructorDescription("Test");
        description.AddMember(c);
        var m = new MethodDescription("void", "Method");
        description.AddMember(m);

        // Act
        var bodies = description.MethodBodies();

        // Assert
        bodies.Should().AllBeAssignableTo<IHaveAMethodBody>();
        bodies.Should().BeEquivalentTo(new IHaveAMethodBody[] { c, m });
    }

    [TestMethod]
    public void TypeDescription_ImplementsType_DoesNotHaveAnyBaseTypes_Should_ReturnFalse()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");

        // Act
        var implementsType = description.ImplementsType("System.Object");

        // Assert
        implementsType.Should().BeFalse();
    }

    [TestMethod]
    public void TypeDescription_ImplementsType_DoesHaveBaseType_Should_ReturnTrue()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");
        description.BaseTypes.Add("System.Object");

        // Act
        var implementsType = description.ImplementsType("System.Object");

        // Assert
        implementsType.Should().BeTrue();
    }

    [TestMethod]
    public void TypeDescription_ImplementsType_DoesNotHaveBaseType_Should_ReturnFalse()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");
        description.BaseTypes.Add("System.Object");

        // Act
        var implementsType = description.ImplementsType("System.Object2");

        // Assert
        implementsType.Should().BeFalse();
    }

    [TestMethod]
    public void TypeDescription_ImplementsTypeStartsWith_DoesNotHaveAnyBaseTypes_Should_ReturnFalse()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");

        // Act
        var implementsType = description.ImplementsTypeStartsWith("System.Object");

        // Assert
        implementsType.Should().BeFalse();
    }

    [TestMethod]
    public void TypeDescription_ImplementsTypeStartsWith_DoesHaveBaseType_Should_ReturnTrue()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");
        description.BaseTypes.Add("System.Object");

        // Act
        var implementsType = description.ImplementsTypeStartsWith("System.");

        // Assert
        implementsType.Should().BeTrue();
    }

    [TestMethod]
    public void TypeDescription_ImplementsTypeStartsWith_DoesNotHaveBaseType_Should_ReturnFalse()
    {
        // Assign
        var description = new TypeDescription(TypeType.Class, "Test");
        description.BaseTypes.Add("System.Object");

        // Act
        var implementsType = description.ImplementsTypeStartsWith("SystemX.");

        // Assert
        implementsType.Should().BeFalse();
    }

    private class UnsupportedMemberDescription : MemberDescription
    {
        public UnsupportedMemberDescription(string name) : base(name)
        {
        }

        public override MemberType MemberType => throw new NotImplementedException();
    }
}
