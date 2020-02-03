using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class ElseStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_ElseStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.ElseStart();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_ElseStart_Should_ContainElseStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ElseStart();

            // Assert
            stringBuilder.ToString().Should().Be("else\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_ElseStart_WithTitle_Should_ContainElseStartLineWithTitle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.ElseStart(text: "Title");

            // Assert
            stringBuilder.ToString().Should().Be("else Title\n");
        }
    }
}
