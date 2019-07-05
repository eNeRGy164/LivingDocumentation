using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class ClassModifierTests
    {
        [TestMethod]
        public void ClassWithoutModifier_Should_HaveDefaultInternalModifier()
        {
            // Assign
            var source = @"
            class Test
            {
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[0].Modifiers.Should().Be(Modifier.Internal);
        }

        [TestMethod]
        public void PublicClass_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            public class Test
            {
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[0].Modifiers.Should().Be(Modifier.Public);
        }

        [TestMethod]
        public void StaticClass_Should_HaveStaticModifier()
        {
            // Assign
            var source = @"
            static class Test
            {
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[0].Modifiers.Should().HaveFlag(Modifier.Static);
        }

        [TestMethod]
        public void NestedClassWithoutModifier_Should_HaveDefaultPrivateModifier()
        {
            // Assign
            var source = @"
            class Test
            {
                class NestedTest
                {
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[1].Modifiers.Should().Be(Modifier.Private);
        }

        [TestMethod]
        public void NestedPublicClass_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            class Test
            {
                public class NestedTest
                {
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[1].Modifiers.Should().Be(Modifier.Public);
        }

        private static IReadOnlyList<TypeDescription> VisitSyntaxTree(string source)
        {
            source.Should().NotBeNullOrWhiteSpace("without source code there is nothing to test");

            var syntaxTree = CSharpSyntaxTree.ParseText(source.Trim());
            var compilation = CSharpCompilation.Create("Test")
                                               .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(syntaxTree);

            var diagnostics = compilation.GetDiagnostics();
            diagnostics.Should().HaveCount(0, "there shoudn't be any compile errors");

            var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

            var types = new List<TypeDescription>();

            var visitor = new SourceAnalyzer(semanticModel, types, Enumerable.Empty<AssemblyIdentity>().ToList());
            visitor.Visit(syntaxTree.GetRoot());

            return types;
        }
    }
}
