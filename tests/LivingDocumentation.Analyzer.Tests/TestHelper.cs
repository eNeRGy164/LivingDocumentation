using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LivingDocumentation.Analyzer.Tests
{
    internal class TestHelper
    {
        public static IReadOnlyList<TypeDescription> VisitSyntaxTree(string source, params string[] ignoreErrorCodes)
        {
            source.Should().NotBeNullOrWhiteSpace("without source code there is nothing to test");

            var syntaxTree = CSharpSyntaxTree.ParseText(source.Trim());
            var compilation = CSharpCompilation.Create("Test")
                                               .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddSyntaxTrees(syntaxTree);

            var diagnostics = compilation.GetDiagnostics().Where(d => !ignoreErrorCodes.Contains(d.Id));
            diagnostics.Should().HaveCount(0, "there shoudn't be any compile errors");

            var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

            var types = new List<TypeDescription>();

            var visitor = new SourceAnalyzer(semanticModel, types);
            visitor.Visit(syntaxTree.GetRoot());

            return types;
        }
    }
}
