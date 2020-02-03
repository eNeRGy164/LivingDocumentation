using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class UmlDiagramStartTests
    {
        [TestMethod]
        public void StringBuilderExtensions_UmlDiagramStart_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.UmlDiagramStart();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_UmlDiagramStart_Should_ContainStartLineWithoutFileName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.UmlDiagramStart();

            // Assert
            stringBuilder.ToString().Should().Be("@startuml\n");
        }

        [TestMethod]
        public void StringBuilderExtensions_UmlDiagramStart_WithFileName_Should_ContainStartLineWithFileName()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.UmlDiagramStart("example.puml");

            // Assert
            stringBuilder.ToString().Should().Be("@startuml example.puml\n");
        }
    }
}
