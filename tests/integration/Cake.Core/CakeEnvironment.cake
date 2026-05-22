#load "./../../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////
// Integration tests for CakeEnvironment (ApplicationRoot from AppContext.BaseDirectory).
// When Assembly.Location is empty (e.g. single-file publish, AOT), ApplicationRoot
// must still be a valid rooted path so script loading and tool resolution work.
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

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.CakeEnvironment")
    .IsDependentOn("Cake.Core.CakeEnvironment.ApplicationRoot.ValidRootedPath");
