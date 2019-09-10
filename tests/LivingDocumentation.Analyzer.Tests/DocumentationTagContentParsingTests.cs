using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class DocumentationTagContentParsingTests
    {

        [TestMethod]
        public void SummaryWithWhitespace_Should_BeTrimmed()
        {
            // Assign
            var source = @"
            /// <summary>
            /// 
            ///   A  
            ///   
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("A");
        }

        [TestMethod]
        public void TagWithParamRefName_Should_HaveNameAsComment()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// A <paramref name=""b"" /> c
                /// </summary>
                public void Method(string b) {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].DocumentationComments.Summary.Should().Be("A b c");
        }

        [TestMethod]
        public void TagWithTypeParamRefName_Should_HaveNameAsComment()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// A <typeparamref name=""b""/> c
                /// </summary>
                public void Method<b>() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].DocumentationComments.Summary.Should().Be("A b c");
        }

        [TestMethod]
        public void SummaryWithLineBreaksBetweenText_Should_HaveOnlySingleSpaceBetweenText()
        {
            // Assign
            var source = @"
            /// <summary>
            /// a
            /// 
            /// b
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("a b");
        }

        [TestMethod]
        public void SummaryWithMixOfTextAndPara_Should_HaveALineBreakBetweenText()
        {
            // Assign
            var source = @"
            /// <summary>
            /// a
            /// <para>b</para>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("a\nb");
        }

        [TestMethod]
        public void SummaryWithMultipleParas_Should_HaveLinebreakBetweenText()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <para>a</para>
            /// <para>b</para>
            /// <para>c</para>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("a\nb\nc");
        }

        [TestMethod]
        public void TagWithASeeWithInnertext_Should_HaveInnerTextAsComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// A <see cref=""Test"">b</see> c
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("A b c");
        }

        [TestMethod]
        public void TagWithSee_Should_HaveCrefAsComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// A <see cref=""b"" /> c
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("A b c");
        }

        [TestMethod]
        public void TagWithCode_Should_HaveInnerTextAsComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// A <c>b</c> c
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("A b c");
        }
    }
}
