using Buildalyzer;
using Buildalyzer.Workspaces;

namespace LivingDocumentation;

public sealed class AnalyzerSetup : IDisposable
{
    public IEnumerable<Project> Projects;
    public readonly Workspace Workspace;

    private AnalyzerSetup(AnalyzerManager Manager)
    {
        this.Workspace = Manager.GetWorkspace();
        this.Projects = this.Workspace.CurrentSolution.Projects;
    }

    public void Dispose()
    {
        this.Workspace.Dispose();
    }

    public static AnalyzerSetup BuildSolutionAnalyzer(string solutionFile, IEnumerable<string> excludedProjects = default!)
    {
        var excludedSet = excludedProjects is not null ? new HashSet<string>(excludedProjects, StringComparer.OrdinalIgnoreCase) : new(0);

        var manager = new AnalyzerManager(solutionFile);
        var analysis = new AnalyzerSetup(manager);

        var assembliesInSolution = analysis.Workspace.CurrentSolution.Projects.Select(p => p.AssemblyName).ToList();

        // Every project in the solution, except unit test projects
        analysis.Projects = analysis.Projects
            .Where(p => !ProjectContainsTestPackageReference(manager, p))
            .Where(p => string.IsNullOrEmpty(p.FilePath) || !excludedSet.Contains(p.FilePath));

        return analysis;
    }

    public static AnalyzerSetup BuildProjectAnalyzer(string projectFile)
    {
        var manager = new AnalyzerManager();
        manager.GetProject(projectFile);

        return new AnalyzerSetup(manager);
    }

    private static bool ProjectContainsTestPackageReference(AnalyzerManager manager, Project p)
    {
        return manager.Projects.First(mp => p.Id.Id == mp.Value.ProjectGuid).Value.ProjectFile.PackageReferences.Any(pr => pr.Name.Contains("Test", StringComparison.Ordinal));
    }
}
