#load "./../utilities/xunit.cake"
#load "./../utilities/paths.cake"

Task("Cake.Common.ReleaseNotesAliases.ParseAllReleaseNotes")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common");
    var file = path.CombineWithFilePath("./ReleaseNotes.md");
    var expectFirst = new ReleaseNotes(
        version: new Version("0.15.2"),
        notes: new [] {
            "Ensured that WiX candle definitions are enclosed in quotes",
            "Corrected issue with WixHeat HarvestType Out parameter"
            },
        rawVersionLine: "### New on 0.15.2 (Released 2016/07/29)"
        );
    var expectLast = new ReleaseNotes(
        version: new Version("0.15.1"),
        notes: new [] {
            "Corrected Issues found with 0.15.0 AppVeyor updates"
            },
        rawVersionLine: "### New on 0.15.1 (Released 2016/07/28)"
        );

    // When
    var result = ParseAllReleaseNotes(file).ToArray();
    var first = result.FirstOrDefault();
    var last = result.LastOrDefault();

    // Then
    Assert.NotNull(first);
    Assert.NotNull(last);
    Assert.NotEqual(first, last);
    Assert.Equal(2, result.Length);

    Assert.Equal(expectFirst.Version, first.Version);
    Assert.Equal(expectFirst.RawVersionLine, first.RawVersionLine);
    Assert.Equal(expectFirst.Notes, first.Notes);

    Assert.Equal(expectLast.Version, last.Version);
    Assert.Equal(expectFirst.RawVersionLine, first.RawVersionLine);
    Assert.Equal(expectFirst.Notes, first.Notes);
});

Task("Cake.Common.ReleaseNotesAliases.ParseReleaseNotes")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common");
    var file = path.CombineWithFilePath("./ReleaseNotes.md");
    var expect = new ReleaseNotes(
        version: new Version("0.15.2"),
        notes: new [] {
            "Ensured that WiX candle definitions are enclosed in quotes",
            "Corrected issue with WixHeat HarvestType Out parameter"
            },
        rawVersionLine: "### New on 0.15.2 (Released 2016/07/29)"
        );

    // When
    var result = ParseReleaseNotes(file);

    // Then
    Assert.NotNull(result);
    Assert.Equal(expect.Version, result.Version);
    Assert.Equal(expect.RawVersionLine, result.RawVersionLine);
    Assert.Equal(expect.Notes, result.Notes);
});

Task("Cake.Common.ReleaseNotesAliases")
    .IsDependentOn("Cake.Common.ReleaseNotesAliases.ParseAllReleaseNotes")
    .IsDependentOn("Cake.Common.ReleaseNotesAliases.ParseReleaseNotes");