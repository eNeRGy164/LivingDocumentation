using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class ClassStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_ClassStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.ClassStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassStart(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.ClassStart(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_Should_ContainClassStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA");

            // Assert
            stringBuilder.ToString().Should().Be("class classA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithDisplayName_Should_ContainClassStartLineWithDisplayName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", displayName: "Class A");

            // Assert
            stringBuilder.ToString().Should().Be("class \"Class A\" as classA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_IsAbstract_Should_ContainClassStartLineWithAbstract()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", isAbstract: true);

            // Assert
            stringBuilder.ToString().Should().Be("abstract class classA {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithStereotype_Should_ContainClassStartLineWithStereotype()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", stereotype: "entity");

            // Assert
            stringBuilder.ToString().Should().Be("class classA <<entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithStereotypeAndCustomSpot_Should_ContainClassStartLineWithStereotypeAndCustomSpot()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", stereotype: "entity", customSpot: new CustomSpot('R', "Blue"));

            // Assert
            stringBuilder.ToString().Should().Be("class classA <<(R,Blue)entity>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithExtends_Should_ContainClassStartLineWithExtends()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", extends: "Object");

            // Assert
            stringBuilder.ToString().Should().Be("class classA<Object> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithBackgroundColor_Should_ContainClassStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", backgroundColor: "#AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("class classA #AliceBlue {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ClassStart_WithBackgroundColorWithHashtag_Should_ContainClassStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ClassStart("classA", backgroundColor: "AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("class classA #AliceBlue {\n");
        }
    }
}
