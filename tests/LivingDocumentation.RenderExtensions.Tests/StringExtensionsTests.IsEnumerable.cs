using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.RenderExtensions.Tests
{
    [TestClass]
    public partial class StringExtensionsTests
    {
        [TestMethod]
        public void IsEnumerable_GenericLinkedList_Should_ReturnTrue()
        {
            // Assign
            var type = "System.Collections.Generic.LinkedList<System.String>";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEnumerable_GenericIList_Should_ReturnTrue()
        {
            // Assign
            var type = "System.Collections.Generic.IList<System.String>";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEnumerable_GenericDictionaryEnumerator_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Generic.Dictionary<System.String,System.String>.Enumerator";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_GenericComparer_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Generic.Comparer<System.String,System.String>";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_GenericKeyNotFoundException_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Generic.KeyNotFoundException";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_ConcurrentConcurrentStack_Should_ReturnTrue()
        {
            // Assign
            var type = "System.Collections.Concurrent.ConcurrentStack<System.String>";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEnumerable_ConcurrentPartitioner_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Concurrent.Partitioner";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_ConcurrentOrderablePartitioner_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Concurrent.OrderablePartitioner<System.String>";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_ArrayList_Should_ReturnTrue()
        {
            // Assign
            var type = "System.Collections.ArrayList";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEnumerable_CaseInsensitiveComparer_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.CaseInsensitiveComparer";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_CaseInsensitiveHashCodeProvider_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.CaseInsensitiveHashCodeProvider";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_Comparer_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.Comparer";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_StructuralComparisons_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Collections.StructuralComparisons";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEnumerable_Object_Should_ReturnFalse()
        {
            // Assign
            var type = "System.Object";

            // Act
            var result = type.IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }
    }
}
