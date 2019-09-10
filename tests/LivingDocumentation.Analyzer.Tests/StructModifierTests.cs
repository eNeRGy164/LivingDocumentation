using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class StructModifierTests
    {
        [TestMethod]
        public void StructWithoutModifier_Should_HaveDefaultInternalModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Modifiers.Should().Be(Modifier.Internal);
        }

        [TestMethod]
        public void PublicStruct_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            public struct Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Modifiers.Should().Be(Modifier.Public);
        }

        [TestMethod]
        public void NestedClassWithoutModifier_Should_HaveDefaultPrivateModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
                struct NestedTest
                {
                }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[1].Modifiers.Should().Be(Modifier.Private);
        }

        [TestMethod]
        public void NestedPublicStruct_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
                public struct NestedTest
                {
                }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[1].Modifiers.Should().Be(Modifier.Public);
        }
    }
}
