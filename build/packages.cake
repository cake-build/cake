public class BuildPackages
{
    public ICollection<BuildPackage> All { get; private set; }
    public ICollection<BuildPackage> NuGet { get; private set; }
    public ICollection<BuildPackage> Chocolatey { get; private set; }

    public static BuildPackages GetPackages(
        DirectoryPath nugetRooPath,
        string semVersion,
        string[] packageIds,
        string[] chocolateyPackageIds)
    {
        var toNuGetPackage = BuildPackage(nugetRooPath, semVersion);
        var toChocolateyPackage = BuildPackage(nugetRooPath, semVersion, isChocolateyPackage: true);
        var nugetPackages = packageIds.Select(toNuGetPackage).ToArray();
        var chocolateyPackages = chocolateyPackageIds.Select(toChocolateyPackage).ToArray();
        
        return new BuildPackages {
            All = nugetPackages.Union(chocolateyPackages).ToArray(),
            NuGet = nugetPackages,
            Chocolatey = chocolateyPackages
        };
    }

    private static Func<string, BuildPackage> BuildPackage(
        DirectoryPath nugetRooPath, 
        string semVersion, 
        bool isChocolateyPackage = false)
    {
        return package => new BuildPackage(
            id: package,
            nuspecPath: string.Concat("./nuspec/", package, ".nuspec"),
            packagePath: nugetRooPath.CombineWithFilePath(string.Concat(package, ".", semVersion, ".nupkg")),
            isChocolateyPackage: isChocolateyPackage);
    }
}

public class BuildPackage
{
    public string Id { get; private set; }
    public FilePath NuspecPath { get; private set; }
    public FilePath PackagePath { get; private set; }
    public bool IsChocolateyPackage { get; private set; }

    public BuildPackage(
        string id,
        FilePath nuspecPath,
        FilePath packagePath,
        bool isChocolateyPackage)
    {
        Id = id;
        NuspecPath = nuspecPath;
        PackagePath = packagePath;
        IsChocolateyPackage = isChocolateyPackage;
    }
}