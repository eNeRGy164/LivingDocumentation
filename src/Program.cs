using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

namespace roslyn_uml
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var types = new List<TypeDescription>();
            
            await AnalyzeSolutionAsync(types, args[0]);

            var aggregateFiles = new eShopOnContainers.AggregateRenderer(types).Render();

            var commandHandlerFiles = new eShopOnContainers.CommandHandlerRenderer(types).Render();

            var eventHandlerFiles = new eShopOnContainers.EventHandlerRenderer(types).Render();

            var asciiDocRenderer = new eShopOnContainers.AsciiDocRenderer(types, aggregateFiles, commandHandlerFiles, eventHandlerFiles);
            asciiDocRenderer.Render();
        }

        private static async Task AnalyzeSolutionAsync(IList<TypeDescription> types, string solutionFile)
        {
            var manager = new AnalyzerManager(solutionFile);
            var workspace = manager.GetWorkspace();
            var assembliesInSolution = workspace.CurrentSolution.Projects.Select(p => p.AssemblyName).ToList();

            // Every project in the solution, except unit test projects
            foreach (var project in workspace.CurrentSolution.Projects.OrderBy(p => p.Name))
            {
                if (project.Documents.Any(a => string.Equals(a.Name, "Microsoft.NET.Test.Sdk.Program.cs")))
                {
                    // Skip test projects
                    continue;
                }

                var compilation = await project.GetCompilationAsync();
                var referencedAssemblies = compilation.ReferencedAssemblyNames.Where(a => !assembliesInSolution.Contains(a.Name)).ToList();

                var diagnostics = compilation.GetDiagnostics();
                if (diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
                {
                    Console.WriteLine($"The following errors occured during compilation of project '{project.FilePath}'");
                    foreach (var diagnostic in diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                    {
                        Console.WriteLine("- " + diagnostic.ToString());
                    }
                }

                // Every file in the project
                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

                    var visitor = new SourceAnalyzer(semanticModel, types, referencedAssemblies);
                    visitor.Visit(syntaxTree.GetRoot());
                }
            }

            workspace.Dispose();
        }
    }
}
