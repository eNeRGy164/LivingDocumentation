using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class DelayTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Delay_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Delay();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Delay_Should_ContainDelayLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Delay();

            // Assert
            stringBuilder.ToString().Should().Be("...\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Delay_WithLabel_Should_ContainDelayLineWithMessage()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Delay("5 minutes later");

            // Assert
            stringBuilder.ToString().Should().Be("...5 minutes later...\n");
        }
    }
}
