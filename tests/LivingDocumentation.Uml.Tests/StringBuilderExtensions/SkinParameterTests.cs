using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class SkinParameterTests
    {
        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.SkinParameter("a", "b");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter(null, "b");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter(string.Empty, "b");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter(" ", "b");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_NullValue_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter("a", null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_EmptyValue_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter("a", string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_WhitespaceValue_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter("a", " ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("value");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_EnumOutOfRange_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.SkinParameter((SkinParameter)int.MaxValue, "b");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A defined enum value should be provided*")
                .And.ParamName.Should().Be("skinParameter");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_Should_ContainSkinParamWithNameAndValue()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.SkinParameter("monochrome", "true");

            // Assert
            stringBuilder.ToString().Should().Be("skinparam monochrome true\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_WhitespaceAroundNameShould_ContainTrimmedName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.SkinParameter(" monochrome ", "true");

            // Assert
            stringBuilder.ToString().Should().Be("skinparam monochrome true\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_WhitespaceAroundValue_Should_ContainTrimmedValue()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.SkinParameter("monochrome", " true ");

            // Assert
            stringBuilder.ToString().Should().Be("skinparam monochrome true\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_SkinParameter_UsingEnumValue_Should_ContainName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.SkinParameter(SkinParameter.Monochrome, "true");

            // Assert
            stringBuilder.ToString().Should().Be("skinparam Monochrome true\n");
        }
    }
}
