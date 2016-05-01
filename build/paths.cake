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
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");
        var testingDir = context.Directory("./src/Cake.Testing/bin") + context.Directory(configuration);

        var cakeAssemblyPaths = new FilePath[] {
            buildDir + context.File("Cake.exe"),
            buildDir + context.File("Cake.Core.dll"),
            buildDir + context.File("Cake.Core.pdb"),
            buildDir + context.File("Cake.Core.xml"),
            buildDir + context.File("Cake.NuGet.dll"),
            buildDir + context.File("Cake.NuGet.pdb"),
            buildDir + context.File("Cake.NuGet.xml"),
            buildDir + context.File("Cake.Common.dll"),
            buildDir + context.File("Cake.Common.pdb"),
            buildDir + context.File("Cake.Common.xml"),
            buildDir + context.File("Mono.CSharp.dll"),
            buildDir + context.File("Autofac.dll"),
            buildDir + context.File("NuGet.Core.dll")
        };

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

        var chocolateyFiles = cakeAssemblyPaths.Concat(new FilePath[] {"LICENSE"})
            .Select(
                file => new ChocolateyNuSpecContent {Source = string.Concat("./../", file.FullPath)}
            ).ToArray();

        var zipArtifactPath = artifactsDir.CombineWithFilePath("Cake-bin-v" + semVersion + ".zip");

        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            artifactsBinDir
            );

        var buildFiles = new BuildFiles(
            context,
            cakeAssemblyPaths,
            testingAssemblyPaths,
            repoFilesPaths,
            artifactSourcePaths,
            zipArtifactPath
            );

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
    public FilePath ZipArtifactPath { get; private set; }

    public BuildFiles(
        ICakeContext context,
        FilePath[] cakeAssemblyPaths,
        FilePath[] testingAssemblyPaths,
        FilePath[] repoFilesPaths,
        FilePath[] artifactsSourcePaths,
        FilePath zipArtifactPath
        )
    {
        CakeAssemblyPaths = Filter(context, cakeAssemblyPaths);
        TestingAssemblyPaths = Filter(context, testingAssemblyPaths);
        RepoFilesPaths = Filter(context, repoFilesPaths);
        ArtifactsSourcePaths = Filter(context, artifactsSourcePaths);
        ZipArtifactPath = zipArtifactPath;
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
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(
        DirectoryPath artifactsDir,
        DirectoryPath testResultsDir,
        DirectoryPath nugetRoot,
        DirectoryPath artifactsBinDir
        )
    {
        Artifacts = artifactsDir;
        TestResults = testResultsDir;
        NugetRoot = nugetRoot;
        ArtifactsBin = artifactsBinDir;
        ToClean = new[] {
            Artifacts,
            TestResults,
            NugetRoot,
            ArtifactsBin
        };
    }
}