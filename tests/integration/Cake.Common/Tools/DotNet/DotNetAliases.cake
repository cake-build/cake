#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

Task("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    var dotnetExePath = Paths.TestRoot.CombineWithFilePath(Context.IsRunningOnUnix() ? ".dotnet/dotnet" : ".dotnet/dotnet.exe");
    if (FileExists(dotnetExePath))
    {
        Context.Tools.RegisterFile(dotnetExePath);
    }

    var sourcePath = Paths.Resources.Combine("./Cake.Common/Tools/DotNet");
    var targetPath = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    EnsureDirectoryExist(targetPath.Combine("../").Collapse());
    CopyDirectory(sourcePath, targetPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRestore")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var root = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");

    // When
    DotNetRestore(root.FullPath, new DotNetRestoreSettings { Verbosity = DotNetVerbosity.Minimal });
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRestore")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/net8.0/hwapp.dll");

    // When
    DotNetBuild(project.FullPath);

    // Then
    Assert.True(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");

    // When
    DotNetTest(project.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetVSTest")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var assembly = path.CombineWithFilePath("hwapp.tests/bin/Debug/net8.0/hwapp.tests.dll");

    // When
    DotNetVSTest(assembly.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTool")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .Does(() =>
{
    // When
    DotNetTool("--info");
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRun")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetVSTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");

    // When
    DotNetRun(project.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPack")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var outputPath = path.Combine("DotNetPack");
    var nuget = outputPath.CombineWithFilePath("hwapp.1.0.0.nupkg");
    var nugetSymbols = outputPath.CombineWithFilePath("hwapp.1.0.0.symbols.nupkg");
    EnsureDirectoryExist(outputPath);

    // When
    DotNetPack(project.FullPath, new DotNetPackSettings { OutputDirectory = outputPath, IncludeSymbols = true });

    // Then
    Assert.True(System.IO.File.Exists(nuget.FullPath), "Path:" + nuget.FullPath);
    Assert.True(System.IO.File.Exists(nugetSymbols.FullPath), "Path:" + nugetSymbols.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetNuGetPush")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPack")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var outputPath = path.Combine("DotNetPack");
    var nugetServerPath = path.Combine("DotNetPush");
    var nugetSource = outputPath.CombineWithFilePath("hwapp.1.0.0.nupkg");
    var nugetDestination = nugetServerPath.CombineWithFilePath("hwapp.1.0.0.nupkg");

    EnsureDirectoryExist(outputPath);
    EnsureDirectoryExist(nugetServerPath);
    Assert.True(System.IO.File.Exists(nugetSource.FullPath), "Path:" + nugetSource.FullPath);

    // When
    DotNetNuGetPush(nugetSource.FullPath, new DotNetNuGetPushSettings { Source = nugetServerPath.FullPath });

    // Then
    Assert.True(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetNuGetDelete")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetNuGetPush")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var nugetServerPath = path.Combine("DotNetPush");
    var nugetDestination = nugetServerPath.CombineWithFilePath("hwapp.1.0.0.nupkg");

    EnsureDirectoryExist(nugetServerPath);
    Assert.True(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);

    // When
    DotNetNuGetDelete("hwapp", "1.0.0", new DotNetNuGetDeleteSettings { Source = nugetServerPath.FullPath, NonInteractive = true });

    // Then
    Assert.False(System.IO.File.Exists(nugetDestination.FullPath), "Path:" + nugetDestination.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublish")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var outputPath = path.Combine("DotNetPublish");
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
    DotNetPublish(project.FullPath, new DotNetPublishSettings { OutputDirectory = outputPath });

    // Then
    foreach(var file in publishFiles)
    {
        Assert.True(System.IO.File.Exists(file.FullPath), "Path:" + file.FullPath);
    }
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetExecute")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/net8.0/hwapp.dll");

    // When
    DotNetExecute(assembly);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetClean")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/net8.0/hwapp.dll");
    Assert.True(System.IO.File.Exists(assembly.FullPath));

    // When
    DotNetClean(project.FullPath);

    // Then
    Assert.False(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetMSBuild")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetClean")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var assembly = path.CombineWithFilePath("hwapp/bin/Debug/net8.0/hwapp.dll");

    // When
    DotNetMSBuild(project.FullPath);

    // Then
    Assert.True(System.IO.File.Exists(assembly.FullPath));
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest.Fail")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");

    // When
    var exception = Record.Exception(()=>DotNetTest(project.FullPath,
        new DotNetTestSettings { EnvironmentVariables = new Dictionary<string, string> {{ "hwapp_fail_test", "true" }}
        }));

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.Equal(exception.Message, ".NET CLI: Process returned an error (exit code 1).");
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetFormat")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");

    // When
    DotNetFormat(project.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetSDKCheck")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // When
    DotNetSDKCheck();
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadSearch")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var searchString = "maui";

    // When
    var workloads = DotNetWorkloadSearch(searchString);

    // Then
    foreach(var workload in workloads)
    {
        Assert.Contains("maui", workload.Id);
        Assert.Contains(".NET MAUI SDK", workload.Description);
    }
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadRepair")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .OnError(exception => { Console.WriteLine(exception); })
    .Does(() =>
{
    // When
    DotNetWorkloadRepair();
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadUpdate")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // When
    DotNetWorkloadUpdate();
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadRestore")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");

    // When
    DotNetWorkloadRestore(project.FullPath);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetAddPackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var package = "grok.net";

    // When
    DotNetAddPackage(package, project.FullPath);

    var value = XmlPeek(
        project.FullPath,
        $"/Project/ItemGroup/PackageReference[@Include='{package}']/@Include"
    );

    // Then
    Assert.Equal(package, value);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRemovePackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var package = "grok.net";
    var value = XmlPeek(
        project.FullPath,
        $"/Project/ItemGroup/PackageReference[@Include='{package}']/@Include"
    );
    Assert.Equal(package, value);

    // When
    DotNetRemovePackage(package, project.FullPath);

    value = XmlPeek(
        project.FullPath,
        $"/Project/ItemGroup/PackageReference[@Include='{package}']/@Include"
    );

    // Then
    Assert.Null(value);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetAddReference")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");
    var projectReferencePath = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var projectReference = "..\\hwapp\\hwapp.csproj";
    // When
    DotNetAddReference(project.FullPath, new[] { (FilePath)projectReferencePath.FullPath});
    var value = XmlPeek(
        project.FullPath,
        $"/Project/ItemGroup/ProjectReference[@Include='{projectReference}']/@Include"
    );
    // Then
    Assert.Equal(projectReference, value);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRemoveReference")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.Setup")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp.tests/hwapp.tests.csproj");
    var projectReferencePath = path.CombineWithFilePath("hwapp/hwapp.csproj");
    var projectReference = "..\\hwapp\\hwapp.csproj";
    // When
    DotNetRemoveReference(project.FullPath, new[] { (FilePath)projectReferencePath.FullPath});
    var value = XmlPeek(
        project.FullPath,
        $"/Project/ItemGroup/ProjectReference[@Include='{projectReference}']/@Include"
    );
    // Then
    Assert.Null(value);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetSearchPackage")
    .Does(() =>
{
    // Given
    var package = "Cake.Tool";

    // When
    var result = DotNetSearchPackage(package);

    // Then
    Assert.NotNull(result);
    Assert.Contains(package, result.Select(x => x.Name));
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetListPackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRestore")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common/Tools/DotNet");
    var project = path.CombineWithFilePath("hwapp/hwapp.csproj");
    // When
    DotNetRestore(project.FullPath);
    var result = DotNetListPackage(project.FullPath);
    // Then
    Assert.Equal(1, result.Version);
    Assert.Contains(result.Projects, item => item.Path == project);
});

Task("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuildServerShutdown")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRestore")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuild")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetVSTest")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTool")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRun")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPack")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetNuGetPush")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetNuGetDelete")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetPublish")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetExecute")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetClean")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetMSBuild")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetTest.Fail")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetFormat")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetSDKCheck")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadSearch")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadRepair")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadUpdate")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetWorkloadRestore")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetAddPackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRemovePackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetSearchPackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetListPackage")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetAddReference")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetRemoveReference")
    .Does(() =>
{
    // When
    DotNetBuildServerShutdown();
});;

Task("Cake.Common.Tools.DotNet.DotNetAliases")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases.DotNetBuildServerShutdown");
