using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class GroupStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_GroupStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.GroupStart();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_GroupStart_Should_ContainGroupStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.GroupStart();

            // Assert
            stringBuilder.ToString().Should().Be("group\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_GroupStart_WithTitle_Should_ContainGroupStartLineWithTitle()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.GroupStart(text: "Title");

            // Assert
            stringBuilder.ToString().Should().Be("group [Title]\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_GroupStart_WithLabel_Should_ContainGroupStartLineWithLabel()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.GroupStart(label: "Label");

            // Assert
            stringBuilder.ToString().Should().Be("group Label\n");
        }
    }
}
