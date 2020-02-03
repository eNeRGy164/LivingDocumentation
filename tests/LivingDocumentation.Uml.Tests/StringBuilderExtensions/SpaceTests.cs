using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class SpaceTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Space_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Space();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Space_Should_ContainSpaceLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Space();

            // Assert
            stringBuilder.ToString().Should().Be("|||\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Space_WithSize_Should_ContainSpaceLineWithSize()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Space(45);

            // Assert
            stringBuilder.ToString().Should().Be("||45||\n");
        }
    }
}
