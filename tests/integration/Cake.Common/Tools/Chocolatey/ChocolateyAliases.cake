#load "./../../../utilities/paths.cake"
#load "./../../../utilities/xunit.cake"

Task("Cake.Common.Tools.Chocolatey.ChocolateyAliases.ChocolateyExport")
    .Does(() => {
    // Given
    var directoryPath = Paths.Temp.Combine("./Cake.Common/Tools/Chocolatey/ChocolateyExport");
    var filePath = directoryPath.CombineWithFilePath("packages.config");
    EnsureDirectoryExists(directoryPath);

    var settings = new ChocolateyExportSettings {
        OutputFilePath = filePath.FullPath
    };

    // When
    ChocolateyExport(settings);

    // Then
    Assert.True(FileExists(filePath), $"{filePath.FullPath} missing");
});


var chocolateyAliasesTask = Task("Cake.Common.Tools.Chocolatey.ChocolateyAliases");

if (IsRunningOnWindows() && Context.Tools.Resolve("choco.exe") is FilePath)
{
    chocolateyAliasesTask
        .IsDependentOn("Cake.Common.Tools.Chocolatey.ChocolateyAliases.ChocolateyExport");
}
