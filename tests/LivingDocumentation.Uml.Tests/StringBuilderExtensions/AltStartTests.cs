using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class AltStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_AltStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.AltStart();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_AltStart_Should_ContainAltStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.AltStart();

            // Assert
            stringBuilder.ToString().Should().Be("alt\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_AltStart_WithTitle_Should_ContainAltStartLineWithTitle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.AltStart(text: "Title");

            // Assert
            stringBuilder.ToString().Should().Be("alt Title\n");
        }
    }
}
