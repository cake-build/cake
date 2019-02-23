public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }

    public static BuildPaths GetPaths(
        ICakeContext context,
        string configuration,
        string semVersion
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        if (string.IsNullOrEmpty(configuration))
        {
            throw new ArgumentNullException("configuration");
        }
        if (string.IsNullOrEmpty(semVersion))
        {
            throw new ArgumentNullException("semVersion");
        }

        var artifactsDir = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var artifactsBinDir = artifactsDir.Combine("bin");
        var artifactsBinFullFx = artifactsBinDir.Combine("net461");
        var artifactsBinNetCore = artifactsBinDir.Combine("netcoreapp2.0");
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");

        var zipArtifactPathCoreClr = artifactsDir.CombineWithFilePath("Cake-bin-coreclr-v" + semVersion + ".zip");
        var zipArtifactPathDesktop = artifactsDir.CombineWithFilePath("Cake-bin-net461-v" + semVersion + ".zip");

        var testCoverageOutputFilePath = testResultsDir.CombineWithFilePath("OpenCover.xml");

        var integrationTestsBin = context.MakeAbsolute(context.Directory("./tests/integration/tools"));
        var integrationTestsBinFullFx = integrationTestsBin.Combine("Cake");
        var integrationTestsBinNetCore = integrationTestsBin.Combine("Cake.CoreCLR");

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            artifactsBinDir,
            artifactsBinFullFx,
            artifactsBinNetCore,
            integrationTestsBinFullFx,
            integrationTestsBinNetCore);

        // Files
        var buildFiles = new BuildFiles(
            context,
            zipArtifactPathCoreClr,
            zipArtifactPathDesktop,
            testCoverageOutputFilePath);

        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories
        };
    }
}

public class BuildFiles
{
    public FilePath ZipArtifactPathCoreClr { get; private set; }
    public FilePath ZipArtifactPathDesktop { get; private set; }
    public FilePath TestCoverageOutputFilePath { get; private set; }

    public BuildFiles(
        ICakeContext context,
        FilePath zipArtifactPathCoreClr,
        FilePath zipArtifactPathDesktop,
        FilePath testCoverageOutputFilePath
        )
    {
        ZipArtifactPathCoreClr = zipArtifactPathCoreClr;
        ZipArtifactPathDesktop = zipArtifactPathDesktop;
        TestCoverageOutputFilePath = testCoverageOutputFilePath;
    }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; }
    public DirectoryPath TestResults { get; }
    public DirectoryPath NuGetRoot { get; }
    public DirectoryPath ArtifactsBin { get; }
    public DirectoryPath ArtifactsBinFullFx { get; }
    public DirectoryPath ArtifactsBinNetCore { get; }
    public DirectoryPath IntegrationTestsBinFullFx { get; }
    public DirectoryPath IntegrationTestsBinNetCore { get; }
    public ICollection<DirectoryPath> ToClean { get; }

    public BuildDirectories(
        DirectoryPath artifactsDir,
        DirectoryPath testResultsDir,
        DirectoryPath nugetRoot,
        DirectoryPath artifactsBinDir,
        DirectoryPath artifactsBinFullFx,
        DirectoryPath artifactsBinNetCore,
        DirectoryPath integrationTestsBinFullFx,
        DirectoryPath integrationTestsBinNetCore
        )
    {
        Artifacts = artifactsDir;
        TestResults = testResultsDir;
        NuGetRoot = nugetRoot;
        ArtifactsBin = artifactsBinDir;
        ArtifactsBinFullFx = artifactsBinFullFx;
        ArtifactsBinNetCore = artifactsBinNetCore;
        IntegrationTestsBinFullFx = integrationTestsBinFullFx;
        IntegrationTestsBinNetCore = integrationTestsBinNetCore;
        ToClean = new[] {
            Artifacts,
            TestResults,
            NuGetRoot,
            ArtifactsBin,
            ArtifactsBinFullFx,
            ArtifactsBinNetCore,
            IntegrationTestsBinFullFx,
            IntegrationTestsBinNetCore
        };
    }
}