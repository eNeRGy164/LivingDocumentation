using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.RenderExtensions.Tests
{
    public partial class StringExtensionsTests
    {
        [TestMethod]
        public void IsGeneric_NonGenericType_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Object";

            // Act
            var result = type.IsGeneric();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsGeneric_GenericType_Should_ReturnTrue()
        {
            // Assign
            var type = "System.Collections.Generic.Dictionary<System.String,System.String>";

            // Act
            var result = type.IsGeneric();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsGeneric_NonGenericEmbeddedType_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Generic.Dictionary<System.String,System.String>.Enumerator";

            // Act
            var result = type.IsGeneric();

            // Assert
            result.Should().BeFalse();
        }
    }
}
