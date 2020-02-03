using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class BoxStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_BoxStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.BoxStart();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_BoxStart_Should_ContainBoxStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.BoxStart();

            // Assert
            stringBuilder.ToString().Should().Be("box\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_BoxStart_WithTitle_Should_ContainBoxStartLineWithTitle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.BoxStart(title: "Box title");

            // Assert
            stringBuilder.ToString().Should().Be("box \"Box title\"\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_BoxStart_WithColor_Should_ContainBoxStartLineWithColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.BoxStart(color: "Blue");

            // Assert
            stringBuilder.ToString().Should().Be("box #Blue\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_BoxStart_WithColorWithHashTag_Should_ContainBoxStartLineWithColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.BoxStart(color: "#Blue");

            // Assert
            stringBuilder.ToString().Should().Be("box #Blue\n");
        }
    }
}
