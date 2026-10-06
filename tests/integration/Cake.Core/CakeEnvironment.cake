#load "./../../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////
// Integration tests for CakeEnvironment (ApplicationRoot from AppContext.BaseDirectory).
// When Assembly.Location is empty (e.g. single-file publish, AOT), ApplicationRoot
// must still be a valid rooted path so script loading and tool resolution work.
// VersionResolver then uses Environment.ProcessPath for FileVersionInfo; CakeRunner
// uses ProcessPath and finally ApplicationRoot to locate Cake.dll.
//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.CakeEnvironment.ApplicationRoot.ValidRootedPath")
    .Does(() =>
{
    // Given - Context.Environment is the real CakeEnvironment used at runtime
    var applicationRoot = Context.Environment.ApplicationRoot;

    // Then - ApplicationRoot must be set and rooted so script loading and tool resolution work
    Assert.NotNull(applicationRoot);
    Assert.NotNull(applicationRoot.FullPath);
    Assert.False(string.IsNullOrWhiteSpace(applicationRoot.FullPath));
    Assert.False(applicationRoot.IsRelative);
});

Task("Cake.Core.CakeEnvironment.AssemblyLocationFallbacks")
    .Does(() =>
{
    // Fallback sources used when Assembly.Location is empty (single-file / AOT).
    Assert.False(string.IsNullOrWhiteSpace(AppContext.BaseDirectory));
    Assert.False(string.IsNullOrWhiteSpace(Environment.ProcessPath));
});

Task("Cake.Core.CakeEnvironment.SpecialPath")
    .Does(context =>
{
    var userProfile = context.Environment.GetSpecialPath(SpecialPath.UserProfile);
    Assert.Equal(context.Environment.UserHomeDirectory.FullPath, userProfile.FullPath);

    if (!context.Environment.Platform.IsUnix())
    {
        return;
    }

    var home = context.Environment.GetEnvironmentVariable("HOME");
    var expectedHome = !string.IsNullOrEmpty(home) ? home : userProfile.FullPath;

    Assert.Equal("/usr/bin", context.Environment.GetSpecialPath(SpecialPath.ProgramFiles).FullPath);
    Assert.Equal("/usr/bin", context.Environment.GetSpecialPath(SpecialPath.ProgramFilesX86).FullPath);
    Assert.Equal(expectedHome, context.Environment.GetSpecialPath(SpecialPath.ApplicationData).FullPath);
    Assert.Equal(expectedHome, context.Environment.GetSpecialPath(SpecialPath.LocalApplicationData).FullPath);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.CakeEnvironment")
    .IsDependentOn("Cake.Core.CakeEnvironment.ApplicationRoot.ValidRootedPath")
    .IsDependentOn("Cake.Core.CakeEnvironment.AssemblyLocationFallbacks")
    .IsDependentOn("Cake.Core.CakeEnvironment.SpecialPath");
