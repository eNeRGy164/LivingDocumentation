using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LivingDocumentation.Uml.Tests
{
    [TestClass]
    public class UmlDiagramEndTests
    {
        [TestMethod]
        public void StringBuilderExtensions_UmlDiagramEnd_Null_Should_ThrowArgumentNullException()
        {
            // Assign
            var stringBuilder = (StringBuilder)null;

            // Act
            Action action = () => stringBuilder.UmlDiagramEnd();

            // Assert
            action.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stringBuilder");
        }

        [TestMethod]
        public void StringBuilderExtensions_UmlDiagramEnd_Should_ContainUmlDiagramEndLine()
        {
            // Assign
            var stringBuilder = new StringBuilder();

            // Act
            stringBuilder.UmlDiagramEnd();

            // Assert
            stringBuilder.ToString().Should().Be("@enduml\n");
        }
    }
}
