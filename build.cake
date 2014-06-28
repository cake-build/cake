// Get arguments passed to the script.
var target = Argument("target", "All");
var configuration = Argument("configuration", "Release");
var teamCity = HasArgument("teamCity");
var buildLabel = Argument("buildLabel", string.Empty);
var buildInfo = Argument("buildInfo", string.Empty);

// Set version.
var version = "0.1.9";
var semVersion = version + (buildLabel != "" ? ("-" + buildLabel) : string.Empty);

// Define directories.
var buildDir = "./src/Cake/bin/" + configuration;
var buildResultDir = "./build/" + "v" + semVersion;
var testResultsDir = buildResultDir + "/test-results";
var nugetRoot = buildResultDir + "/nuget";
var binDir = buildResultDir + "/bin";

//////////////////////////////////////////////////////////////////////////

Task("Update-TeamCity-Build-Number")
	.WithCriteria(teamCity)
	.Does(() =>
{
	Console.WriteLine("##teamcity[buildNumber '%s']", semVersion);
});

Task("Clean")
	.IsDependentOn("Update-TeamCity-Build-Number")
	.Does(() =>
{
	CleanDirectories(new DirectoryPath[] {
		buildResultDir, binDir, testResultsDir, nugetRoot});
});

Task("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(() =>
{
	NuGetRestore("./src/Cake.sln");
});

Task("Patch-Assembly-Info")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	var file = "./src/SolutionInfo.cs";
	CreateAssemblyInfo(file, new AssemblyInfoSettings {
		Product = "Cake",
		Version = version,
		FileVersion = version,
		InformationalVersion = (version + buildInfo).Trim(),
		Copyright = "Copyright (c) Patrik Svensson 2014"
	});
});

Task("Build")
	.IsDependentOn("Patch-Assembly-Info")
	.Does(() =>
{
	MSBuild("./src/Cake.sln", settings =>
		settings.SetConfiguration(configuration)
			.UseToolVersion(MSBuildToolVersion.NET45));
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Copy-Files")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
{
    CopyFileToDirectory(buildDir + "/Cake.exe", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Common.dll", binDir);
    CopyFileToDirectory(buildDir + "/NuGet.Core.dll", binDir);
    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
});

Task("Zip-Files")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	var filename = buildResultDir + "/Cake-bin-v" + version + ".zip";
	Zip(binDir, filename);
});

Task("Create-NuGet-Package")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	NuGetPack("./Cake.nuspec", new NuGetPackSettings {
		Version = version,
        BasePath = binDir,
        OutputDirectory = nugetRoot,
        Symbols = true,
        NoPackageAnalysis = true
	});
});

Task("Package")
	.IsDependentOn("Zip-Files")
	.IsDependentOn("Create-NuGet-Package");

Task("All")
	.IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////////

RunTarget(target);