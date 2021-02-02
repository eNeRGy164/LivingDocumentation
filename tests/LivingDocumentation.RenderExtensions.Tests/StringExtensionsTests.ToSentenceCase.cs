using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LivingDocumentation.RenderExtensions.Tests
{
    public partial class StringExtensionsTests
    {
        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithPascalCaseSingleWord_Should_ReturnSameString()
        {
            // Assign
            var type = "Object";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Object");
        }

        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithPascalCaseMultipleWord_Should_ReturnSpaceInjectedBeforeEachWord()
        {
            // Assign
            var type = "InvocationsAnalyzer";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Invocations Analyzer");
        }

        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithAbbrevation_Should_ReturnWithAbbrevationIntact()
        {
            // Assign
            var type = "InvocationsAnalyzerAPI";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Invocations Analyzer API");
        }

        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithAbbrevationAndMoreText_Should_ReturnWithAbbrevationIntact()
        {
            // Assign
            var type = "InvocationsAnalyzerAPIService";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Invocations Analyzer API Service");
        }

        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithNumbers_Should_ReturnWithNumbersCombined()
        {
            // Assign
            var type = "InvocationsAnalyzer164";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Invocations Analyzer 164");
        }

        [TestMethod]
        public void GenericTypes_ToSentenceCaseWithNumbersAndMoreText_Should_ReturnWithNumbersCombined()
        {
            // Assign
            var type = "InvocationsAnalyzer164Class";

            // Act
            var result = type.ToSentenceCase();

            // Assert
            result.Should().Be("Invocations Analyzer 164 Class");
        }
    }
}
