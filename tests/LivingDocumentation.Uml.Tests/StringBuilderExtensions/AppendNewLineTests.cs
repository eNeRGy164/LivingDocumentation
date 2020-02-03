using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class AppendNewLineTests
    {
        [TestMethod]
        public void StringBuilderExtensions_AppendNewLine_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.AppendNewLine();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_AppendNewLine_Should_ContainNewLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.AppendNewLine();

            // Assert
            stringBuilder.ToString().Should().Be("\n");
        }
    }
}
