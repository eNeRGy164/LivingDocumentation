using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class StructModifierTests
    {
        [TestMethod]
        public void StructWithoutModifier_Should_HaveDefaultInternalModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types.First().Modifiers.Should().HaveCount(1);
            types.First().Modifiers.Should().Contain("internal");
        }

        [TestMethod]
        public void PublicStruct_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            public struct Test
            {
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types.First().Modifiers.Should().HaveCount(1);
            types.First().Modifiers.Should().Contain("public");
        }

        [TestMethod]
        public void NestedClassWithoutModifier_Should_HaveDefaultPrivateModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
                struct NestedTest
                {
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types.Last().Modifiers.Should().HaveCount(1);
            types.Last().Modifiers.Should().Contain("private");
        }

        [TestMethod]
        public void NestedPublicStruct_Should_HavePublicModifier()
        {
            // Assign
            var source = @"
            struct Test
            {
                public struct NestedTest
                {
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types.Last().Modifiers.Should().HaveCount(1);
            types.Last().Modifiers.Should().Contain("public");
        }

        private static IReadOnlyList<TypeDescription> VisitSyntaxTree(string source)
        {
            source.Should().NotBeNull();

            var syntaxTree = CSharpSyntaxTree.ParseText(source.Trim());
            var compilation = CSharpCompilation.Create("Test")
                                               .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(syntaxTree);

            var diagnostics = compilation.GetDiagnostics();
            diagnostics.Should().HaveCount(0);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

            var types = new List<TypeDescription>();

            var visitor = new SourceAnalyzer(semanticModel, types, Enumerable.Empty<AssemblyIdentity>().ToList());
            visitor.Visit(syntaxTree.GetRoot());

            return types;
        }
    }
}
