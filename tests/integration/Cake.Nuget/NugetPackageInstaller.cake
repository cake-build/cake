Task("Cake.Nuget.Install.NugetPackageInstaller.VersionPinFull")
    .Does(()=>
{
    var script = $@"#addin nuget:?package=Serilog&version=2.7.1
        
        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion; 

        Information(""Requested nuget version: 2.7.1"");
        Information(""Loaded nuget version: "" + fileVersion);
        
        if(!string.Equals(fileVersion, ""2.7.1.0""))
            throw new Exception(""VersionPinFull failed"");
    ";

    CakeExecuteExpression(script,
        new CakeSettings {
            Verbosity = Context.Log.Verbosity
        });
});

Task("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.InclusiveExclusive")
    .Does(()=>
{
    var script = $@"#addin nuget:?package=Serilog&version=[2.5.0,2.6.0)
        
        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion; 

        Information(""Requested nuget version: [2.5.0,2.6.0)"");
        Information(""Loaded nuget version: "" + fileVersion);
        
        var parsedVersion = Version.Parse(fileVersion);

        if(parsedVersion < new Version(2, 5, 0) || parsedVersion >= new Version(2, 6, 0))
            throw new Exception(""VersionPinRange.InclusiveExclusive failed"");
    ";

    CakeExecuteExpression(script,
        new CakeSettings {
            Verbosity = Context.Log.Verbosity
        });
});

Task("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.InclusiveInclusive")
    .Does(()=>
{
    var script = $@"#addin nuget:?package=Serilog&version=[2.3.0,2.4.0]
        
        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion; 

        Information(""Requested nuget version: [2.3.0,2.4.0]"");
        Information(""Loaded nuget version: "" + fileVersion);
        
        var parsedVersion = Version.Parse(fileVersion);

        if(parsedVersion < new Version(2, 3, 0) || parsedVersion > new Version(2, 4, 0))
            throw new Exception(""VersionPinRange.InclusiveInclusive failed"");
    ";

    CakeExecuteExpression(script,
        new CakeSettings {
            Verbosity = Context.Log.Verbosity
        });
});

Task("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.Wildcard")
    .Does(()=>
{
    var script = $@"#addin nuget:?package=Serilog&version=[2.2.*,2.3.0)
        
        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion; 

        Information(""Requested nuget version: [2.2.*,2.3.0)"");
        Information(""Loaded nuget version: "" + fileVersion);
        
        var parsedVersion = Version.Parse(fileVersion);

        if(parsedVersion < new Version(2, 2, 0) || parsedVersion >= new Version(2, 3, 0))
            throw new Exception(""VersionPinRange.Wildcard failed"");
    ";

    CakeExecuteExpression(script,
        new CakeSettings {
            Verbosity = Context.Log.Verbosity
        });
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Nuget.NugetPackageInstaller")
    .IsDependentOn("Cake.Nuget.Install.NugetPackageInstaller.VersionPinFull")
    .IsDependentOn("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.InclusiveExclusive")
    .IsDependentOn("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.InclusiveInclusive")
    .IsDependentOn("Cake.Nuget.Install.NugetPackageInstaller.VersionPinRange.Wildcard");