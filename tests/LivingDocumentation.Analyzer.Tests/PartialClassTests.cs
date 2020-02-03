using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class PartialClassTests
    {
        [TestMethod]
        public void ClassWithParts_Should_HaveSingleEntry()
        {
            // Assign
            var source = @"
            partial class Test
            {
            }

            partial class Test
            {
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types.Should().HaveCount(1);
        }

        [TestMethod]
        public void ClassWithParts_Should_HaveCombinedProperties()
        {
            // Assign
            var source = @"
            partial class Test
            {
                public string Property1 { get; }
            }

            partial class Test
            {
                public string Property2 { get; }
            }
            ";

            // Act
            var types = TestHelper.VisitSyntaxTree(source);

            // Assert
            types[0].Properties.Should().HaveCount(2);
        }

    }
}
