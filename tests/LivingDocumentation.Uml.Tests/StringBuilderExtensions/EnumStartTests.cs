using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class EnumStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_EnumStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.EnumStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.EnumStart(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.EnumStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.EnumStart(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_Should_ContainEnumStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA");

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithDisplayName_Should_ContainEnumStartLineWithDisplayName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", displayName: "Enum A");

            // Assert
            stringBuilder.ToString().Should().Be("enum \"Enum A\" as enumA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithStereotype_Should_ContainEnumStartLineWithStereotype()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", stereotype: "entity");

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA <<entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithStereotypeAndCustomSpot_Should_ContainEnumStartLineWithStereotypeAndCustomSpot()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", stereotype: "entity", customSpot: new CustomSpot('R', "Blue"));

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA <<(R,Blue)entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithGenerics_Should_ContainEnumStartLineWithExtends()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", generics: "Object");

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA<Object> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithBackgroundColor_Should_ContainEnumStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", backgroundColor: "#AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA #AliceBlue {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumStart_WithBackgroundColorWithHashtag_Should_ContainEnumStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumStart("enumA", backgroundColor: "AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("enum enumA #AliceBlue {\n");
        }
    }
}
