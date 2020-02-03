using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class DividerTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Divider_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Divider();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Divider_Should_ContainDividerLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Divider();

            // Assert
            stringBuilder.ToString().Should().Be("====\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Divider_WithLabel_Should_ContainDividerLineWithTitle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Divider("Initialization");

            // Assert
            stringBuilder.ToString().Should().Be("==Initialization==\n");
        }
    }
}
