public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }
    public ICollection<ChocolateyNuSpecContent> ChocolateyFiles { get; private set; }

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

        var buildDir = context.Directory("./src/Cake/bin") + context.Directory(configuration);
        var artifactsDir = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var artifactsBinDir = artifactsDir.Combine("bin");
        var artifactsBinNet45 = artifactsBinDir.Combine("net45");
        var artifactsBinNetCoreApp10 = artifactsBinDir.Combine("netcoreapp1.0");
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");
        var testingDir = context.Directory("./src/Cake.Testing/bin") + context.Directory(configuration);

        var cakeFiles = new FilePath[] {
            context.File("Cake.exe"),
            context.File("Cake.pdb"),
            context.File("Cake.Core.dll"),
            context.File("Cake.Core.pdb"),
            context.File("Cake.Core.xml"),
            context.File("Cake.NuGet.dll"),
            context.File("Cake.NuGet.pdb"),
            context.File("Cake.NuGet.xml"),
            context.File("Cake.Common.dll"),
            context.File("Cake.Common.pdb"),
            context.File("Cake.Common.xml"),
            context.File("Mono.CSharp.dll"),
            context.File("Autofac.dll"),
            context.File("NuGet.Core.dll")
        };

        var cakeAssemblyPaths = cakeFiles.Concat(new FilePath[] {"LICENSE"})
            .Select(file => buildDir.Path.CombineWithFilePath(file))
            .ToArray();

        var testingAssemblyPaths = new FilePath[] {
            testingDir + context.File("Cake.Testing.dll"),
            testingDir + context.File("Cake.Testing.pdb"),
            testingDir + context.File("Cake.Testing.xml")
        };

        var repoFilesPaths = new FilePath[] {
            "LICENSE",
            "README.md",
            "ReleaseNotes.md"
        };

        var artifactSourcePaths = cakeAssemblyPaths.Concat(testingAssemblyPaths.Concat(repoFilesPaths)).ToArray();

        var relPath = new DirectoryPath("./").MakeAbsolute(context.Environment).GetRelativePath(artifactsBinNet45.MakeAbsolute(context.Environment));
        var chocolateyFiles = cakeFiles.Concat(new FilePath[] {"LICENSE"})
            .Select(file => new ChocolateyNuSpecContent {Source = "../" + relPath.CombineWithFilePath(file).FullPath})
            .ToArray();

        var zipArtifactPathCoreClr = artifactsDir.CombineWithFilePath("Cake-bin-coreclr-v" + semVersion + ".zip");
        var zipArtifactPathDesktop = artifactsDir.CombineWithFilePath("Cake-bin-net45-v" + semVersion + ".zip");
        var zipArtifactPathTestResults = artifactsDir.CombineWithFilePath("TestResults-v" + semVersion + ".zip");

        var testCoverageOutputFilePath = testResultsDir.CombineWithFilePath("OpenCover.xml");

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            artifactsBinDir,
            artifactsBinNet45,
            artifactsBinNetCoreApp10);

        // Files
        var buildFiles = new BuildFiles(
            context,
            cakeAssemblyPaths,
            testingAssemblyPaths,
            repoFilesPaths,
            artifactSourcePaths,
            zipArtifactPathCoreClr,
            zipArtifactPathDesktop,
            testCoverageOutputFilePath,
            zipArtifactPathTestResults);

        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories,
            ChocolateyFiles = chocolateyFiles
        };
    }
}

public class BuildFiles
{
    public ICollection<FilePath> CakeAssemblyPaths { get; private set; }
    public ICollection<FilePath> TestingAssemblyPaths { get; private set; }
    public ICollection<FilePath> RepoFilesPaths { get; private set; }
    public ICollection<FilePath> ArtifactsSourcePaths { get; private set; }
    public FilePath ZipArtifactPathCoreClr { get; private set; }
    public FilePath ZipArtifactPathDesktop { get; private set; }
    public FilePath TestCoverageOutputFilePath { get; private set; }
    public FilePath ZipArtifactPathTestResults { get; private set; }

    public BuildFiles(
        ICakeContext context,
        FilePath[] cakeAssemblyPaths,
        FilePath[] testingAssemblyPaths,
        FilePath[] repoFilesPaths,
        FilePath[] artifactsSourcePaths,
        FilePath zipArtifactPathCoreClr,
        FilePath zipArtifactPathDesktop,
        FilePath testCoverageOutputFilePath,
        FilePath zipArtifactPathTestResults
        )
    {
        CakeAssemblyPaths = Filter(context, cakeAssemblyPaths);
        TestingAssemblyPaths = Filter(context, testingAssemblyPaths);
        RepoFilesPaths = Filter(context, repoFilesPaths);
        ArtifactsSourcePaths = Filter(context, artifactsSourcePaths);
        ZipArtifactPathCoreClr = zipArtifactPathCoreClr;
        ZipArtifactPathDesktop = zipArtifactPathDesktop;
        TestCoverageOutputFilePath = testCoverageOutputFilePath;
        ZipArtifactPathTestResults = zipArtifactPathTestResults;
    }

    private static FilePath[] Filter(ICakeContext context, FilePath[] files)
    {
        // Not a perfect solution, but we need to filter PDB files
        // when building on an OS that's not Windows (since they don't exist there).
        if(!context.IsRunningOnWindows())
        {
            return files.Where(f => !f.FullPath.EndsWith("pdb")).ToArray();
        }
        return files;
    }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; private set; }
    public DirectoryPath TestResults { get; private set; }
    public DirectoryPath NugetRoot { get; private set; }
    public DirectoryPath ArtifactsBin { get; private set; }
    public DirectoryPath ArtifactsBinNet45 { get; private set; }
    public DirectoryPath ArtifactsBinNetCoreApp10 { get; private set; }
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(
        DirectoryPath artifactsDir,
        DirectoryPath testResultsDir,
        DirectoryPath nugetRoot,
        DirectoryPath artifactsBinDir,
        DirectoryPath artifactsBinNet45,
        DirectoryPath artifactsBinNetCoreApp10
        )
    {
        Artifacts = artifactsDir;
        TestResults = testResultsDir;
        NugetRoot = nugetRoot;
        ArtifactsBin = artifactsBinDir;
        ArtifactsBinNet45 = artifactsBinNet45;
        ArtifactsBinNetCoreApp10 = artifactsBinNetCoreApp10;
        ToClean = new[] {
            Artifacts,
            TestResults,
            NugetRoot,
            ArtifactsBin,
            ArtifactsBinNet45,
            ArtifactsBinNetCoreApp10
        };
    }
}