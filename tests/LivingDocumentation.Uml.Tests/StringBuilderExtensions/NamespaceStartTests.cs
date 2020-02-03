using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class NamespaceStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.NamespaceStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_NullName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.NamespaceStart(null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_EmptyName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.NamespaceStart(string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_WhitespaceName_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.NamespaceStart(" ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_Should_ContainNamespaceStartLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.NamespaceStart("namespace");

            // Assert
            stringBuilder.ToString().Should().Be("namespace namespace {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_WithDisplayName_Should_ContainNamespaceStartLineWithDisplayName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.NamespaceStart("namespace", displayName: "Name space");

            // Assert
            stringBuilder.ToString().Should().Be("namespace \"Name space\" as namespace {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_WithStereotype_Should_ContainNamespaceStartLineWithStereotype()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.NamespaceStart("namespace", stereotype: "assembly");

            // Assert
            stringBuilder.ToString().Should().Be("namespace namespace <<assembly>> {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_WithBackgroundColor_Should_ContainNamespaceStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.NamespaceStart("namespace", backgroundColor: "#AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("namespace namespace #AliceBlue {\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_NamespaceStart_WithBackgroundColorWithHashtag_Should_ContainNamespaceStartLineWithBackgroundColor()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.NamespaceStart("namespace", backgroundColor: "AliceBlue");

            // Assert
            stringBuilder.ToString().Should().Be("namespace namespace #AliceBlue {\n");
        }
    }
}
