using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using CommandLine;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace LivingDocumentation
{
    public static partial class Program
    {
        private static ParserResult<Options> ParsedResults;

        public static Options RuntimeOptions { get; private set; } = new Options();

        public static async Task Main(string[] args)
        {
            ParsedResults = Parser.Default.ParseArguments<Options>(args);

            await ParsedResults.MapResult(
                options => RunApplicationAsync(options),
                errors => Task.FromResult(1)
            ).ConfigureAwait(false);
        }

        private static async Task RunApplicationAsync(Options options)
        {
            RuntimeOptions = options;

            var types = new List<TypeDescription>();

            var stopwatch = Stopwatch.StartNew();
            await AnalyzeSolutionAsync(types, options.SolutionPath).ConfigureAwait(false);
            stopwatch.Stop();

            // Write analysis 
            var serializerSettings = JsonDefaults.SerializerSettings();
            serializerSettings.Formatting = options.PrettyPrint ? Formatting.Indented : Formatting.None;

            var result = JsonConvert.SerializeObject(types.OrderBy(t => t.FullName), serializerSettings);

            await File.WriteAllTextAsync(options.OutputPath, result).ConfigureAwait(false);

            if (!options.Quiet)
            {
                Console.WriteLine($"Living Documentation Analysis output generated in {stopwatch.ElapsedMilliseconds}ms at {options.OutputPath}");
            }
        }

        private static async Task AnalyzeSolutionAsync(IList<TypeDescription> types, string solutionFile)
        {
            var manager = new AnalyzerManager(solutionFile);
            var workspace = manager.GetWorkspace();
            var assembliesInSolution = workspace.CurrentSolution.Projects.Select(p => p.AssemblyName).ToList();

            // Every project in the solution, except unit test projects
            var projects = workspace.CurrentSolution.Projects
                .Where(p => !manager.Projects.First(mp => p.Id.Id == mp.Value.ProjectGuid).Value.ProjectFile.PackageReferences.Any(pr => pr.Name.Contains("Test", StringComparison.Ordinal)));

            foreach (var project in projects)
            {
                var compilation = await project.GetCompilationAsync().ConfigureAwait(false);

                if (RuntimeOptions.VerboseOutput)
                {
                    var diagnostics = compilation.GetDiagnostics();
                    if (diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
                    {
                        Console.WriteLine($"The following errors occured during compilation of project '{project.FilePath}'");
                        foreach (var diagnostic in diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                        {
                            Console.WriteLine("- " + diagnostic.ToString());
                        }
                    }
                }

                // Every file in the project
                foreach (var syntaxTree in compilation.SyntaxTrees)
                {
                    var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

                    var visitor = new SourceAnalyzer(semanticModel, types);
                    visitor.Visit(syntaxTree.GetRoot());
                }
            }

            workspace.Dispose();
        }
    }
}
