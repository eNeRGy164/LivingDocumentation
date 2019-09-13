using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class DocumentationTagContentParsingTests
    {
        [TestMethod]
        public void Summary_Should_BeRead()
        {
            // Assign
            var source = @"
            /// <summary>
            /// A summary.
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("A summary.");
        }

        [TestMethod]
        public void Remarks_Should_BeRead()
        {
            // Assign
            var source = @"
            /// <remarks>
            /// A remark.
            /// </remarks>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Remarks.Should().Be("A remark.");
        }

        [TestMethod]
        public void Exception_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <exception cref=""System.Exception"">An exception.</exception>
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Exceptions.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.Exceptions.Should().Contain("System.Exception", "An exception."); 
            }
        }

        [TestMethod]
        public void Exceptions_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <exception cref=""System.Exception"">An exception.</exception>
                /// <exception cref=""System.StackOverflowException"">Another exception.</exception>
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Exceptions.Should().HaveCount(2);
                types[0].Methods[0].DocumentationComments.Exceptions.Should().Contain("System.Exception", "An exception.");
                types[0].Methods[0].DocumentationComments.Exceptions.Should().Contain("System.StackOverflowException", "Another exception.");
            }
        }

        [TestMethod]
        public void Param_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <param name=""param"">This is a param.</param>
                void Method(string param) {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Params.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.Params.Should().Contain("param", "This is a param.");
            }
        }

        [TestMethod]
        public void Params_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <param name=""param1"">This is the first parameter.</param>
                /// <param name=""param2"">This is the second parameter.</param>
                void Method(string param1, string param2) {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Params.Should().HaveCount(2);
                types[0].Methods[0].DocumentationComments.Params.Should().Contain("param1", "This is the first parameter.");
                types[0].Methods[0].DocumentationComments.Params.Should().Contain("param2", "This is the second parameter.");
            }
        }

        [TestMethod]
        public void Permission_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <permission cref=""System.Security.PermissionSet"">A permission.</permission>
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Permissions.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.Permissions.Should().Contain("System.Security.PermissionSet", "A permission.");
            }
        }

        [TestMethod]
        public void Permissions_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <permission cref=""System.Security.Permissions.EnvironmentPermission"">A permission.</permission>
                /// <permission cref=""System.Security.Permissions.FileIOPermission"">Another permission.</permission>
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.Permissions.Should().HaveCount(2);
                types[0].Methods[0].DocumentationComments.Permissions.Should().Contain("System.Security.Permissions.EnvironmentPermission", "A permission.");
                types[0].Methods[0].DocumentationComments.Permissions.Should().Contain("System.Security.Permissions.FileIOPermission", "Another permission.");
            }
        }

        [TestMethod]
        public void SeeAlso_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <seealso cref=""System.String"" />
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().Contain("System.String", "System.String");
            }
        }

        [TestMethod]
        public void SeeAlsos_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <seealso cref=""System.String"" />
                /// <seealso cref=""System.Object"">See also.</seealso>
                void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().HaveCount(2);
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().Contain("System.String", "System.String");
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().Contain("System.Object", "See also.");
            }
        }

        [TestMethod]
        public void TypeParam_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <typeparam name=""T"">This is a type param.</typeparam>
                void Method<T>() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.TypeParams.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.TypeParams.Should().Contain("T", "This is a type param.");
            }
        }

        [TestMethod]
        public void TypeParams_Should_BeRead()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <typeparam name=""T1"">This is the first type parameter.</typeparam>
                /// <typeparam name=""T2"">This is the second type parameter.</typeparam>
                void Method<T1, T2>() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.TypeParams.Should().HaveCount(2);
                types[0].Methods[0].DocumentationComments.TypeParams.Should().Contain("T1", "This is the first type parameter.");
                types[0].Methods[0].DocumentationComments.TypeParams.Should().Contain("T2", "This is the second type parameter.");
            }
        }

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
        public void TagWithSeeAlso_Should_IgnoreSeeAlso()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// <seealso cref=""System.Object"">See also.</seealso>
                /// </summary>
                public void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].DocumentationComments.Summary.Should().BeEmpty();
        }

        [TestMethod]
        public void TagWithSeeAlso_Should_AddToSeeAlsos()
        {
            // Assign
            var source = @"
            class Test
            {
                /// <summary>
                /// <seealso cref=""System.Object"">See also.</seealso>
                /// </summary>
                public void Method() {}
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            using (new AssertionScope())
            {
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().HaveCount(1);
                types[0].Methods[0].DocumentationComments.SeeAlsos.Should().Contain("System.Object", "See also.");
            }
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
        public void TagWithSeeWithInnertext_Should_HaveInnerTextAsComment()
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

        [TestMethod]
        public void TagWithInvalidList_Should_HaveListItemsAsInlineTextInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list>
            /// <item>First item</item>
            /// <item>Second item</item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("First item Second item");
        }

        [TestMethod]
        public void TagWithBulletList_Should_HaveListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""bullet"">
            /// <item>First item</item>
            /// <item>Second item</item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("* First item\n* Second item");
        }

        [TestMethod]
        public void TagWithBulletListWithDescriptions_Should_HaveListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""bullet"">
            /// <item><description>First item</description></item>
            /// <item><description>Second item</description></item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("* First item\n* Second item");
        }

        [TestMethod]
        public void TagWithBulletListWithTermsAndDescriptions_Should_HaveListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""bullet"">
            /// <item>
            /// <term>Term 1</term>
            /// <description>First item</description>
            /// </item>
            /// <item>
            /// <term>Term 2</term>
            /// <description>Second item</description>
            /// </item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("* Term 1 - First item\n* Term 2 - Second item");
        }



        [TestMethod]
        public void TagWithNumberedList_Should_HaveNumberedListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""number"">
            /// <item>First item</item>
            /// <item>Second item</item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("1. First item\n2. Second item");
        }

        [TestMethod]
        public void TagWithNumberedListWithStart_Should_HaveCorrectNumberedListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""number"" start=""3"">
            /// <item>First item</item>
            /// <item>Second item</item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("3. First item\n4. Second item");
        }

        [TestMethod]
        public void TagWithNumberedListWithDescriptions_Should_HaveNumberedListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""number"">
            /// <item><description>First item</description></item>
            /// <item><description>Second item</description></item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("1. First item\n2. Second item");
        }

        [TestMethod]
        public void TagWithNumberedListWithTermsAndDescriptions_Should_HaveNumberedListItemsAsLinesInComment()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""number"">
            /// <item>
            /// <term>Term 1</term>
            /// <description>First item</description>
            /// </item>
            /// <item>
            /// <term>Term 2</term>
            /// <description>Second item</description>
            /// </item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("1. Term 1 - First item\n2. Term 2 - Second item");
        }

        [TestMethod]
        public void TagWithDefinitionListWithTermsAndDescriptions_Should_HaveSeperateLinesWithTermAndDescriptionIndented()
        {
            // Assign
            var source = @"
            /// <summary>
            /// <list type=""definition"">
            /// <item>
            /// <term>Term 1</term>
            /// <description>First item</description>
            /// </item>
            /// <item>
            /// <term>Term 2</term>
            /// <description>Second item</description>
            /// </item>
            /// </list>
            /// </summary>
            class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].DocumentationComments.Summary.Should().Be("Term 1\n    First item\nTerm 2\n    Second item");
        }
    }
}
