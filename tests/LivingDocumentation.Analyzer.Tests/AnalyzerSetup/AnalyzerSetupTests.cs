namespace LivingDocumentation.Analyzer.Tests;

[TestClass]
public class AnalyzerSetupTests
{
    private static readonly string solutionPath = GetSolutionPath();

    [TestMethod]
    public void SolutionShouldLoadAllProjects()
    {
        // Arrange
        var solutionFile = Path.Combine(solutionPath, "SolutionWithoutTests.sln");

        // Act
        using var analyzerSetup = AnalyzerSetup.BuildSolutionAnalyzer(solutionFile);

        // Assert
        analyzerSetup.Projects.Should().HaveCount(3);
        analyzerSetup.Projects.Should().Satisfy(
            p => p.FilePath.EndsWith("Project.csproj"),
            p => p.FilePath.EndsWith("OtherProject.csproj"),
            p => p.FilePath.EndsWith("AnotherProject.csproj"));
    }

    [TestMethod]
    public void SolutionShouldFilterTestProjects()
    {
        // Arrange
        var solutionFile = Path.Combine(solutionPath, "SolutionWithTests.sln");

        // Act
        using var analyzerSetup = AnalyzerSetup.BuildSolutionAnalyzer(solutionFile);

        // Assert
        analyzerSetup.Projects.Should().HaveCount(3);
        analyzerSetup.Projects.Should().Satisfy(
            p => p.FilePath.EndsWith("Project.csproj"),
            p => p.FilePath.EndsWith("OtherProject.csproj"),
            p => p.FilePath.EndsWith("AnotherProject.csproj"));
    }

    [TestMethod]
    public void SolutionShouldFilterExcludedProject()
    {
        // Arrange
        var solutionFile = Path.Combine(solutionPath, "SolutionWithoutTests.sln");
        var excludeProjectFile = Path.Combine(solutionPath, "OtherProject", "OtherProject.csproj");

        // Act
        using var analyzerSetup = AnalyzerSetup.BuildSolutionAnalyzer(solutionFile, new[] { excludeProjectFile });

        // Assert
        analyzerSetup.Projects.Should().HaveCount(2);
        analyzerSetup.Projects.Should().Satisfy(
            p => p.FilePath.EndsWith("Project.csproj"),
            p => p.FilePath.EndsWith("AnotherProject.csproj"));
    }

    [TestMethod]
    public void SolutionShouldFilterExcludedProjects()
    {
        // Arrange
        var solutionFile = Path.Combine(solutionPath, "SolutionWithoutTests.sln");
        var excludeProjectFile1 = Path.Combine(solutionPath, "OtherProject", "OtherProject.csproj");
        var excludeProjectFile2 = Path.Combine(solutionPath, "AnotherProject", "AnotherProject.csproj");

        // Act
        using var analyzerSetup = AnalyzerSetup.BuildSolutionAnalyzer(solutionFile, new[] { excludeProjectFile1, excludeProjectFile2 });

        // Assert
        analyzerSetup.Projects.Should().HaveCount(1);
        analyzerSetup.Projects.Should().Satisfy(p => p.FilePath.EndsWith("Project.csproj"));
    }

    [TestMethod]
    public void SolutionShouldLoadProject()
    {
        // Arrange
        var projectFile = Path.Combine(solutionPath, "Project", "Project.csproj");

        // Act
        using var analyzerSetup = AnalyzerSetup.BuildProjectAnalyzer(projectFile);

        // Assert
        analyzerSetup.Projects.Should().HaveCount(1);
        analyzerSetup.Projects.Should().Satisfy(p => p.FilePath.EndsWith("Project.csproj"));
    }

    private static string GetSolutionPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory().AsSpan();

        var path = currentDirectory[..(currentDirectory.IndexOf("tests") + 6)];

        return Path.Combine(path.ToString(), "AnalyzerSetupVerification");
    }
}
