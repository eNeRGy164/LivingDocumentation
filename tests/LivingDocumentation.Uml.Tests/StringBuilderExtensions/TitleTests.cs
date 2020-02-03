using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class TitleTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Title_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Title("Title");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Title_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Title(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("title");
        }

        [TestMethod]
        public void StringBuilderExtensions_Title_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Title(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("title");
        }

        [TestMethod]
        public void StringBuilderExtensions_Title_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Title(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("title");
        }

        [TestMethod]
        public void StringBuilderExtensions_Title_Should_ContainTitleLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Title("Title");

            // Assert
            stringBuilder.ToString().Should().Be("title Title\n");
        }
    }
}
