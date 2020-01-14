using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class AttributeTests
    {
        [TestMethod]
        public void ClassWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void ClassWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            [System.Obsolete]
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Attributes.Should().HaveCount(1);
            types[0].Attributes[0].Should().NotBeNull();
            types[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }

        [TestMethod]
        public void MethodWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void MethodWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                [System.Obsolete]
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].Attributes.Should().HaveCount(1);
            types[0].Methods[0].Attributes[0].Should().NotBeNull();
            types[0].Methods[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }
    }
}
