using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.RenderExtensions.Tests
{
    public partial class StringExtensionsTests
    {
        [TestMethod]
        public void GenericTypes_NonGenericType_Should_ReturnEmptyList()
        {
            // Assign
            var type = "System.Object";

            // Act
            var result = type.GenericTypes();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GenericTypes_GenericList_Should_ReturnGenericType()
        {
            // Assign
            var type = "System.Collections.Generic.List<System.String>";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.String" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericListWithWhitespace_Should_ReturnGenericType()
        {
            // Assign
            var type = "System.Collections.Generic.List< System.String >";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.String" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericDictionary_Should_ReturnKeyAndValueType()
        {
            // Assign
            var type = "System.Collections.Generic.Dictionary<System.String,System.String>";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.String", "System.String" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericDictionaryWithWhitespace_Should_ReturnKeyAndValueType()
        {
            // Assign
            var type = "System.Collections.Generic.Dictionary< System.String , System.String >";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.String", "System.String" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericInGeneric_Should_ReturnCorrectGeneric()
        {
            // Assign
            var type = "System.Collections.Generic.List<System.Nullable<System.Int32>>";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.Nullable<System.Int32>" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericInGenericWithWhitespace_Should_ReturnCorrectGeneric()
        {
            // Assign
            var type = "System.Collections.Generic.List< System.Nullable< System.Int32 > >";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.Nullable< System.Int32 >" };
            result.Should().Equal(expectedTypes);
        }

        [TestMethod]
        public void GenericTypes_GenericWithDictionaryInGeneric_Should_ReturnGenericDictionary()
        {
            // Assign
            var type = "System.Nullable<System.Collections.Generic.Dictionary<System.Int32,System.Object>>";

            // Act
            var result = type.GenericTypes();

            // Assert
            var expectedTypes = new[] { "System.Collections.Generic.Dictionary<System.Int32,System.Object>" };
            result.Should().Equal(expectedTypes);
        }
    }
}
