using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace LivingDocumentation.Analyzer.Tests
{
    [TestClass]
    public class ForEachTests
    {
        [TestMethod]
        public void ForEach_Should_BeDetected()
        {
            // Assign
            var source = @"
            class Test
            {
                void Method()
                {
                    var items = new string[0];
                    foreach (var item in items)
                    {
                    }
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            types[0].Methods[0].Statements[0].Should().BeOfType<ForEach>();
        }

        [TestMethod]
        public void ForEachStatements_Should_BeParsed()
        {
            // Assign
            var source = @"
            class Test
            {
                void Method()
                {
                    var items = new string[0];
                    foreach (var item in items)
                    {
                        var o = new System.Object();
                    }
                }
            }
            ";

            // Act
            var types = VisitSyntaxTree(source);

            // Assert
            var forEach = types[0].Methods[0].Statements[0];
            forEach.Statements.Should().HaveCount(1);
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
