// Get arguments passed to the script.
var target = Argument("target", "All");
var configuration = Argument("configuration", "Release");
var teamCity = HasArgument("teamCity");
var buildLabel = Argument("buildLabel", string.Empty);
var buildInfo = Argument("buildInfo", string.Empty);

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Set version.
var version = releaseNotes.Version.ToString();
var semVersion = version + (buildLabel != "" ? ("-" + buildLabel) : string.Empty);
Information("Building version {0} of Cake.", version);

// Define directories.
var buildDir = "./src/Cake/bin/" + configuration;
var buildResultDir = "./build/" + "v" + semVersion;
var testResultsDir = buildResultDir + "/test-results";
var nugetRoot = buildResultDir + "/nuget";
var binDir = buildResultDir + "/bin";

//////////////////////////////////////////////////////////////////////////

Task("Update-TeamCity-Build-Number")
	.Description("Updates the TeamCity build number.")
	.WithCriteria(teamCity)
	.Does(() =>
{
	Console.WriteLine("##teamcity[buildNumber '{0}']", semVersion);
});

Task("Clean")
	.Description("Cleans the build and output directories.")
	.IsDependentOn("Update-TeamCity-Build-Number")
	.Does(() =>
{
	CleanDirectories(new DirectoryPath[] {
		buildResultDir, binDir, testResultsDir, nugetRoot});
});

Task("Restore-NuGet-Packages")
	.Description("Restores all NuGet packages in solution.")
	.IsDependentOn("Clean")
	.Does(() =>
{
	NuGetRestore("./src/Cake.sln");
});

Task("Patch-Assembly-Info")
	.Description("Patches the AssemblyInfo files.")
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
	.Description("Builds the Cake source code.")
	.IsDependentOn("Patch-Assembly-Info")
	.Does(() =>
{
	MSBuild("./src/Cake.sln", settings =>
		settings.SetConfiguration(configuration)
			.UseToolVersion(MSBuildToolVersion.NET45));
});

Task("Run-Unit-Tests")
	.Description("Runs unit tests.")
	.IsDependentOn("Build")
	.Does(() =>
{
	XUnit("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnitSettings {
		OutputDirectory = testResultsDir,
		XmlReport = true,
		HtmlReport = true
	});
});


Task("Copy-Files")
	.Description("Copy files to the output directory.")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
{
	CopyFileToDirectory(buildDir + "/Cake.exe", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.xml", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.pdb", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Common.dll", binDir);
    CopyFileToDirectory(buildDir + "/Autofac.dll", binDir);
    CopyFileToDirectory(buildDir + "/Nuget.Core.dll", binDir);
    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
});

Task("Zip-Files")
	.Description("Zips all files.")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	var filename = buildResultDir + "/Cake-bin-v" + version + ".zip";
	Zip(binDir, filename);
});

Task("Create-Cake-NuGet-Package")
	.Description("Creates the Cake NuGet package.")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	NuGetPack("./Cake.nuspec", new NuGetPackSettings {
		Version = version,
		ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,        
        Symbols = false,
        NoPackageAnalysis = true
	});
});

Task("Create-Core-NuGet-Package")
	.Description("Creates the Cake NuGet package.")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	NuGetPack("./Cake.Core.nuspec", new NuGetPackSettings {
		Version = version,
		ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,        
        Symbols = true
	});
});

Task("Package")
	.Description("Zips and creates NuGet package.")
	.IsDependentOn("Zip-Files")
	.IsDependentOn("Create-Cake-NuGet-Package")
	.IsDependentOn("Create-Core-NuGet-Package");

Task("All")
	.Description("Final target.")
	.IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////////

RunTarget(target);