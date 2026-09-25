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

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.CakeEnvironment")
    .IsDependentOn("Cake.Core.CakeEnvironment.ApplicationRoot.ValidRootedPath")
    .IsDependentOn("Cake.Core.CakeEnvironment.AssemblyLocationFallbacks");
