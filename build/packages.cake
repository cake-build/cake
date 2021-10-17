public record BuildPackages(
    ICollection<BuildPackage> NuGet
)
{
    public static BuildPackages GetPackages(
        DirectoryPath nugetRooPath,
        string semVersion,
        string[] packageIds)
    {
        var toNuGetPackage = BuildPackage(nugetRooPath, semVersion);
        var nugetPackages = packageIds.Select(toNuGetPackage).ToArray();

        return new BuildPackages(nugetPackages);
    }

    private static Func<string, BuildPackage> BuildPackage(
        DirectoryPath nugetRooPath,
        string semVersion)
    {
        return package => new BuildPackage(
            Id: package,
            PackagePath: nugetRooPath.CombineWithFilePath(string.Concat(package, ".", semVersion, ".nupkg"))
            );
    }
}

public record BuildPackage(
        string Id,
        FilePath PackagePath
);