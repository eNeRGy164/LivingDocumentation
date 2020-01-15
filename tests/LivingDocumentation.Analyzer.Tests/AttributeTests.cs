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
        public void EnumWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            enum Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void EnumWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            [System.Obsolete]
            enum Test
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
        public void InterfaceWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            interface Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void InterfaceWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            [System.Obsolete]
            interface Test
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
        public void StructWithoutAttributes_Should_HaveEmptyAttributeCollection()
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
            types[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void StructWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            [System.Obsolete]
            struct Test
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

        [TestMethod]
        public void ConstructorWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                Test() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Constructors[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void ConstructorWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                [System.Obsolete]
                Test() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Constructors[0].Attributes.Should().HaveCount(1);
            types[0].Constructors[0].Attributes[0].Should().NotBeNull();
            types[0].Constructors[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }

        [TestMethod]
        public void FieldWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            public class Test
            {
                public int field;
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Fields[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void FieldWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            public class Test
            {
                [System.Obsolete]
                public int field;
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Fields[0].Attributes.Should().HaveCount(1);
            types[0].Fields[0].Attributes[0].Should().NotBeNull();
            types[0].Fields[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }

        [TestMethod]
        public void EventWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                event System.Action @event;

                Test() { @event(); }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Events[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void EventWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                [System.Obsolete]
                event System.Action @event;

                Test() { @event(); }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Events[0].Attributes.Should().HaveCount(1);
            types[0].Events[0].Attributes[0].Should().NotBeNull();
            types[0].Events[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }

        [TestMethod]
        public void PropertyWithoutAttributes_Should_HaveEmptyAttributeCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                int Property { get; set; }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Properties[0].Attributes.Should().BeEmpty();
        }

        [TestMethod]
        public void PropertyWithAttribute_Should_HaveAttributeInCollection()
        {
            // Assign
            var source = @"
            class Test
            {
                [System.Obsolete]
                int Property { get; set; }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Properties[0].Attributes.Should().HaveCount(1);
            types[0].Properties[0].Attributes[0].Should().NotBeNull();
            types[0].Properties[0].Attributes[0].Name.Should().Be("System.Obsolete");
        }
    }
}
