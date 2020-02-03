using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class EnumEndTests
    {
        [TestMethod]
        public void StringBuilderExtensions_EnumEnd_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.EnumEnd();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_EnumEnd_Should_ContainEndEnum()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.EnumEnd();

            // Assert
            stringBuilder.ToString().Should().Be("}\n");
        }
    }
}
