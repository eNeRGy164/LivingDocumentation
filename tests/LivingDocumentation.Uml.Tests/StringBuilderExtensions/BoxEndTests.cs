using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class BoxEndTests
    {
        [TestMethod]
        public void StringBuilderExtensions_BoxEnd_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.BoxEnd();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_BoxEnd_Should_ContainBoxEndLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.BoxEnd();

            // Assert
            stringBuilder.ToString().Should().Be("end box\n");
        }
    }
}
