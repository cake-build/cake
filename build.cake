#l "utilities.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

// Get arguments passed to the script.
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Get whether or not this is a local build.
var local = IsLocalBuild();
var isPullRequest = IsPullRequest();

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Get version.
var buildNumber = GetBuildNumber();
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

// Define directories.
var buildDir = "./src/Cake/bin/" + configuration;
var buildResultDir = "./build/v" + semVersion;
var testResultsDir = buildResultDir + "/test-results";
var nugetRoot = buildResultDir + "/nuget";
var binDir = buildResultDir + "/bin";

// Output some information about the current build.
Information("Building version {0} of Cake ({1}).", version, semVersion);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
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
		InformationalVersion = semVersion,
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
	XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings {
		OutputDirectory = testResultsDir,
		XmlReportV1 = true
	});	
});


Task("Copy-Files")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
{
	CopyFileToDirectory(buildDir + "/Cake.exe", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.xml", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Core.pdb", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Common.dll", binDir);
    CopyFileToDirectory(buildDir + "/Cake.Common.xml", binDir);
    CopyFileToDirectory(buildDir + "/Autofac.dll", binDir);
    CopyFileToDirectory(buildDir + "/Nuget.Core.dll", binDir);
    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
});

Task("Zip-Files")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	var filename = buildResultDir + "/Cake-bin-v" + semVersion + ".zip";
	Zip(binDir, filename);
});

Task("Create-Cake-NuGet-Package")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	NuGetPack("./Cake.nuspec", new NuGetPackSettings {
		Version = semVersion,
		ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,        
        Symbols = false,
        NoPackageAnalysis = true
	});
});

Task("Create-Core-NuGet-Package")
	.IsDependentOn("Copy-Files")
	.Does(() =>
{
	NuGetPack("./Cake.Core.nuspec", new NuGetPackSettings {
		Version = semVersion,
		ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,        
        Symbols = true
	});
});

Task("Package")
	.IsDependentOn("Zip-Files")
	.IsDependentOn("Create-Cake-NuGet-Package")
	.IsDependentOn("Create-Core-NuGet-Package");

Task("Update-AppVeyor-Build-Number")
	.WithCriteria(() => !local)
	.Does(() =>
{
	SetBuildVersion(semVersion);
});	

Task("Upload-AppVeyor-Artifacts")
	.IsDependentOn("Package")
	.WithCriteria(() => !local)	
	.Does(() =>
{
	// Upload zip file.
	var filename = new FilePath(buildResultDir + "/Cake-bin-v" + semVersion + ".zip");
	UploadArtifact(filename);
});	

Task("Publish-MyGet")
	.WithCriteria(() => !local && !isPullRequest)
	.Does(() =>
{
	// Resolve the API key.
	var apiKey = EnvironmentVariable("MYGET_API_KEY");
	if(string.IsNullOrEmpty(apiKey)) {
		throw new InvalidOperationException("Could not resolve MyGet API key.");
	}

	// Get the path to the package.
	var package = nugetRoot + "/Cake." + semVersion + ".nupkg";

	// Push the package.
	NuGetPush(package, new NuGetPushSettings {
		Source = "https://www.myget.org/F/cake/api/v2/package",
		ApiKey = apiKey
	});	
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Package");

Task("AppVeyor")
	.IsDependentOn("Update-AppVeyor-Build-Number")
	.IsDependentOn("Upload-AppVeyor-Artifacts")
	.IsDependentOn("Publish-MyGet");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);