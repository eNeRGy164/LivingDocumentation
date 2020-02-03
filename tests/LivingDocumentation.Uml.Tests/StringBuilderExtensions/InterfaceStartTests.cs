using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class InterfaceStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.InterfaceStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.InterfaceStart(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.InterfaceStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.InterfaceStart(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_Should_ContainInterfaceStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA");

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithDisplayName_Should_ContainInterfaceStartLineWithDisplayName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", displayName: "Interface A");

            // Assert
            stringBuilder.ToString().Should().Be("interface \"Interface A\" as interfaceA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithStereotype_Should_ContainInterfaceStartLineWithStereotype()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", stereotype: "entity");

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA <<entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithStereotypeAndCustomSpot_Should_ContainInterfaceStartLineWithStereotypeAndCustomSpot()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", stereotype: "entity", customSpot: new CustomSpot('R', "Blue"));

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA <<(R,Blue)entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithGenerics_Should_ContainInterfaceStartLineWithGenerics()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", generics: "Object");

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA<Object> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithBackgroundColor_Should_ContainInterfaceStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", backgroundColor: "#AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA #AliceBlue {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_InterfaceStart_WithBackgroundColorWithHashtag_Should_ContainInterfaceStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.InterfaceStart("interfaceA", backgroundColor: "AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("interface interfaceA #AliceBlue {\n");
        }
    }
}
