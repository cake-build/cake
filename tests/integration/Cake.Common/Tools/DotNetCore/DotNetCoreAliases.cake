#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.Setup")
    .Does(() =>
{
    var dotnetExePath = Paths.TestRoot.CombineWithFilePath(Context.IsRunningOnUnix() ? ".dotnet/dotnet" : ".dotnet/dotnet.exe");
    if (FileExists(dotnetExePath))
    {
        Context.Tools.RegisterFile(dotnetExePath);
    }

    var sourcePath = Paths.Resources.Combine("./Cake.Common/Tools/DotNetCore");
    var targetPath = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    EnsureDirectoryExist(targetPath.Combine("../").Collapse());
    CopyDirectory(sourcePath, targetPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreRestore")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.Setup")
    .Does(() =>
{
    // Given
    var root = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");

    // When
    DotNetCoreRestore(root.FullPath, new DotNetCoreRestoreSettings { Verbosity = DotNetCoreVerbosity.Minimal });
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreRestore")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/netcoreapp3.1/hwapp.dll");

    // When
    DotNetCoreBuild(project.FullPath);

    // Then
    Assert.True(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");

    // When
    DotNetCoreTest(project.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreVSTest")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var assembly = path.CombineWithFilePath("hwapp.tests/bin/Debug/netcoreapp3.1/hwapp.tests.dll");

    // When
    DotNetCoreVSTest(assembly.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTool")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .Does(() =>
{
    // When
    DotNetCoreTool("--info");
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreRun")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreVSTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");

    // When
    DotNetCoreRun(project.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCorePack")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var outputPath = path.Combine("DotNetCorePack");
    var nuget = outputPath.CombineWithFilePath("hwapp.1.0.0.nupkg");
    var nugetSymbols = outputPath.CombineWithFilePath("hwapp.1.0.0.symbols.nupkg");
    EnsureDirectoryExist(outputPath);

    // When
    DotNetCorePack(project.FullPath, new DotNetCorePackSettings { OutputDirectory = outputPath, IncludeSymbols = true });

    // Then
    Assert.True(System.IO.File.Exists(nuget.FullPath), "Path:" + nuget.FullPath);
    Assert.True(System.IO.File.Exists(nugetSymbols.FullPath), "Path:" + nugetSymbols.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreNuGetPush")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCorePack")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var outputPath = path.Combine("DotNetCorePack");
    var nugetServerPath = path.Combine("DotNetCorePush");
    var nugetSource = outputPath.CombineWithFilePath("hwapp.1.0.0.nupkg");
    var nugetDestination = nugetServerPath.CombineWithFilePath("hwapp.1.0.0.nupkg");

    EnsureDirectoryExist(outputPath);
    EnsureDirectoryExist(nugetServerPath);
    Assert.True(System.IO.File.Exists(nugetSource.FullPath), "Path:" + nugetSource.FullPath);

    // When
    DotNetCoreNuGetPush(nugetSource.FullPath, new DotNetCoreNuGetPushSettings { Source = nugetServerPath.FullPath });

    // Then
    Assert.True(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreNuGetDelete")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreNuGetPush")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var nugetServerPath = path.Combine("DotNetCorePush");
    var nugetDestination = nugetServerPath.CombineWithFilePath("hwapp.1.0.0.nupkg");

    EnsureDirectoryExist(nugetServerPath);
    Assert.True(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);

    // When
    DotNetCoreNuGetDelete("hwapp", "1.0.0", new DotNetCoreNuGetDeleteSettings { Source = nugetServerPath.FullPath, NonInteractive = true });

    // Then
    Assert.False(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCorePublish")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var outputPath = path.Combine("DotNetCorePublish");
    var publishFiles = new [] {
        outputPath.CombineWithFilePath("hwapp.common.dll"),
        outputPath.CombineWithFilePath("hwapp.common.pdb"),
        outputPath.CombineWithFilePath("hwapp.deps.json"),
        outputPath.CombineWithFilePath("hwapp.dll"),
        outputPath.CombineWithFilePath("hwapp.pdb"),
        outputPath.CombineWithFilePath("hwapp.runtimeconfig.json")
    };

    EnsureDirectoryExist(outputPath);

    // When
    DotNetCorePublish(project.FullPath, new DotNetCorePublishSettings { OutputDirectory = outputPath });

    // Then
    foreach(var file in publishFiles)
    {
        Assert.True(System.IO.File.Exists(file.FullPath), "Path:" + file.FullPath);
    }
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreExecute")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/netcoreapp3.1/hwapp.dll");

    // When
    DotNetCoreExecute(assembly);
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreClean")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/netcoreapp3.1/hwapp.dll");
    Assert.True(System.IO.File.Exists(assembly.FullPath));

    // When
    DotNetCoreClean(project.FullPath);

    // Then
    Assert.False(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreMSBuild")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreClean")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/netcoreapp3.1/hwapp.dll");

    // When
    DotNetCoreMSBuild(project.FullPath);

    // Then
    Assert.True(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest.Fail")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNetCore");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");

    // When
    var exception = Record.Exception(()=>DotNetCoreTest(project.FullPath,
        new DotNetCoreTestSettings { EnvironmentVariables = new Dictionary<string, string> {{ "hwapp_fail_test", "true" }}
        }));

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.Equal(exception.Message, ".NET Core CLI: Process returned an error (exit code 1).");
});


Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuildServerShutdown")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreRestore")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuild")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreVSTest")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTool")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreRun")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCorePack")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreNuGetPush")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreNuGetDelete")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCorePublish")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreExecute")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreClean")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreMSBuild")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreTest.Fail")
    .Does(() =>
{
    // When
    DotNetCoreBuildServerShutdown();
});;

Task("Cake.Common.Tools.DotNetCore.DotNetCoreAliases")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases.DotNetCoreBuildServerShutdown");