using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class DocumentationCommentsTests
    {
        [TestMethod]
        public void ClassWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            /// <summary>
            /// This is a Test Class.
            /// </summary>
            /// <remarks>
            /// This is a remark.
            /// </remarks>
            /// <example>
            /// This is an example.
            /// </example>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].DocumentationComments.Summary.Should().Be("This is a Test Class.");
                types[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].DocumentationComments.Example.Should().Be("This is an example.");
            }
        }

        [TestMethod]
        public void EventWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// This is a Test Event.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <value>
                /// This is the value.
                /// </value>
                event System.Action @event;

                Test() { @event(); }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Events[0].DocumentationComments.Summary.Should().Be("This is a Test Event.");
                types[0].Events[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Events[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Events[0].DocumentationComments.Value.Should().Be("This is the value.");
            }
        }

        [TestMethod]
        public void MultipleEventDeclarationsWithComments_Should_HaveDocumentationParsedForEveryEvent()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// These are Test Events.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <value>
                /// This is the value.
                /// </value>
                event System.Action event1, event2;

                Test() { event1(); event2(); }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Events[0].DocumentationComments.Summary.Should().Be("These are Test Events.");
                types[0].Events[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Events[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Events[0].DocumentationComments.Value.Should().Be("This is the value.");

                types[0].Events[1].DocumentationComments.Summary.Should().Be("These are Test Events.");
                types[0].Events[1].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Events[1].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Events[1].DocumentationComments.Value.Should().Be("This is the value.");
            }
        }

        [TestMethod]
        public void FieldWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            public class Test
            {
                /// <summary>
                /// This is a Test Field.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <value>
                /// This is the value.
                /// </value>
                public string field;
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Fields[0].DocumentationComments.Summary.Should().Be("This is a Test Field.");
                types[0].Fields[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Fields[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Fields[0].DocumentationComments.Value.Should().Be("This is the value.");
            }
        }

        [TestMethod]
        public void MultipleFieldDeclarationsWithComments_Should_HaveDocumentationParsedForEveryField()
        {
            // Assign
            var source = @"
            public class Test
            {
                /// <summary>
                /// These are Test Fields.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <value>
                /// This is the value.
                /// </value>
                public string field1, field2;
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Fields[0].DocumentationComments.Summary.Should().Be("These are Test Fields.");
                types[0].Fields[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Fields[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Fields[0].DocumentationComments.Value.Should().Be("This is the value.");

                types[0].Fields[1].DocumentationComments.Summary.Should().Be("These are Test Fields.");
                types[0].Fields[1].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Fields[1].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Fields[1].DocumentationComments.Value.Should().Be("This is the value.");
            }
        }

        [TestMethod]
        public void InterfaceWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            /// <summary>
            /// This is a Test Interface.
            /// </summary>
            /// <remarks>
            /// This is a remark.
            /// </remarks>
            /// <example>
            /// This is an example.
            /// </example>
            interface Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].DocumentationComments.Summary.Should().Be("This is a Test Interface.");
                types[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].DocumentationComments.Example.Should().Be("This is an example.");
            }
        }

        [TestMethod]
        public void EnumWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            /// <summary>
            /// This is a Test Enum.
            /// </summary>
            /// <remarks>
            /// This is a remark.
            /// </remarks>
            /// <example>
            /// This is an example.
            /// </example>
            enum Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].DocumentationComments.Summary.Should().Be("This is a Test Enum.");
                types[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].DocumentationComments.Example.Should().Be("This is an example.");
            }
        }

        [TestMethod]
        public void StructWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            /// <summary>
            /// This is a Test Struct.
            /// </summary>
            /// <remarks>
            /// This is a remark.
            /// </remarks>
            /// <example>
            /// This is an example.
            /// </example>
            struct Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].DocumentationComments.Summary.Should().Be("This is a Test Struct.");
                types[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].DocumentationComments.Example.Should().Be("This is an example.");
            }
        }

        [TestMethod]
        public void MethodWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// This is a Test Method.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <returns>
                /// Returns a string.
                /// </returns>
                public string Method() { return null; }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Summary.Should().Be("This is a Test Method.");
                types[0].Methods[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Methods[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Methods[0].DocumentationComments.Returns.Should().Be("Returns a string.");
            }
        }

        [TestMethod]
        public void ConstructorWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// This is a Test Constructor.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <returns>
                /// Returns a string.
                /// </returns>
                Test() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Constructors[0].DocumentationComments.Summary.Should().Be("This is a Test Constructor.");
                types[0].Constructors[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Constructors[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Constructors[0].DocumentationComments.Returns.Should().Be("Returns a string.");
            }
        }

        [TestMethod]
        public void PropertyWithComments_Should_HaveDocumentationParsed()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// This is a Test Property.
                /// </summary>
                /// <remarks>
                /// This is a remark.
                /// </remarks>
                /// <example>
                /// This is an example.
                /// </example>
                /// <value>
                /// This is the value.
                /// </value>
                public string Property { get; set; }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Properties[0].DocumentationComments.Summary.Should().Be("This is a Test Property.");
                types[0].Properties[0].DocumentationComments.Remarks.Should().Be("This is a remark.");
                types[0].Properties[0].DocumentationComments.Example.Should().Be("This is an example.");
                types[0].Properties[0].DocumentationComments.Value.Should().Be("This is the value.");
            }
        }
    }
}
