using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class RelationshipTests
    {
        [TestMethod]
        public void StringBuilderExtensions_Relationship_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.Relationship("l", "-", "r");

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_NullLeft_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship(null, "-", "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("left");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_EmptyLeft_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship(string.Empty, "-", "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("left");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WhitespaceLeft_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship(" ", "-", "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("left");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_NullType_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", null, "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("type");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_EmptyType_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", string.Empty, "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("type");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WhitespaceType_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", " ", "r");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("type");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_NullRight_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", "-", null);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("right");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_EmptyRight_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", "-", string.Empty);

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("right");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WhitespaceRight_Should_ThrowArgumentException()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            Action action = () => stringBuilder.Relationship("l", "-", " ");

            // Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("A non-empty value should be provided*")
                .And.ParamName.Should().Be("right");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_Should_ContainRelationshipLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Relationship("l", "-", "r");

            // Assert
            stringBuilder.ToString().Should().Be("l - r\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WithLabel_Should_ContainRelationshipLineWithLabel()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Relationship("l", "-", "r", label: "label1");

            // Assert
            stringBuilder.ToString().Should().Be("l - r : label1\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WithLeftCardinality_Should_ContainRelationshipLineWithLeftCardinality()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Relationship("l", "-", "r", leftCardinality: "*");

            // Assert
            stringBuilder.ToString().Should().Be("l \"*\" - r\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_Relationship_WithRightCardinality_Should_ContainRelationshipLineWithRightCardinality()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.Relationship("l", "-", "r", rightCardinality: "*");

            // Assert
            stringBuilder.ToString().Should().Be("l - \"*\" r\n");
        }
    }
}
