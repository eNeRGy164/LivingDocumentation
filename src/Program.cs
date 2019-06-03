using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace roslyn_uml
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var types = new List<TypeDescription>();
            
            await AnalyzeSolutionAsync(types, args[0]);

            // Write analysis 
            var serializerSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                Formatting = Formatting.None,
                ContractResolver = new SkipEmptyCollectionsContractResolver(),
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto
            };

            var result = JsonConvert.SerializeObject(types.OrderBy(t => t.FullName), serializerSettings);
            await File.WriteAllTextAsync("analysis.json", result);

            var aggregateFiles = new eShopOnContainers.AggregateRenderer(types).Render();
            var commandHandlerFiles = new eShopOnContainers.CommandHandlerRenderer(types).Render();
            var eventHandlerFiles = new eShopOnContainers.EventHandlerRenderer(types).Render();

            new eShopOnContainers.AsciiDocRenderer(types, aggregateFiles, commandHandlerFiles, eventHandlerFiles).Render();
        }

        private static async Task AnalyzeSolutionAsync(IList<TypeDescription> types, string solutionFile)
        {
            var manager = new AnalyzerManager(solutionFile);
            var workspace = manager.GetWorkspace();
            var assembliesInSolution = workspace.CurrentSolution.Projects.Select(p => p.AssemblyName).ToList();

            // Every project in the solution, except unit test projects
            var projects = workspace.CurrentSolution.Projects
                .Where(p => !manager.Projects.First(mp => p.Id.Id == mp.Value.ProjectGuid).Value.ProjectFile.PackageReferences.Any(pr => pr.Name.Contains("Test")));

            foreach (var project in projects)
            {
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
