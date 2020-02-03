using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class GroupEndTests
    {
        [TestMethod]
        public void StringBuilderExtensions_GroupEnd_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.GroupEnd();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_GroupEnd_Should_ContainNewLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.GroupEnd();

            // Assert
            stringBuilder.ToString().Should().Be("end\n");
        }
    }
}
