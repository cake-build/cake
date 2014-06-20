// Load the FAKE assembly.
#r @"tools/fake/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// Get the release notes.
// It's here we will get stuff like version number from.
let releaseNotes = 
    ReadFile "ReleaseNotes.md"
    |> ReleaseNotesHelper.parseReleaseNotes

// Set the build mode (default to release).
let buildMode = getBuildParamOrDefault "buildMode" "Release"
let teamCity = hasBuildParam "teamCity"

let buildLabel = getBuildParamOrDefault "buildLabel" ""
let buildInfo =   if(hasBuildParam "buildInfo") then (getBuildParam "buildInfo") else ""
let version = releaseNotes.AssemblyVersion
let semVersion = releaseNotes.AssemblyVersion + (if buildLabel <> "" then ("-" + buildLabel) else "")

// Define directories.
let buildDir = "./src/Cake/bin" @@ buildMode
let buildResultDir = "./build" @@ "v" + semVersion + "/"
let testResultsDir = buildResultDir @@ "test-results"
let nugetRoot = buildResultDir @@ "nuget/"
let binDir = buildResultDir @@ "bin/"

////////////////////////////////////////////////////////////////////
// Define helper function for TeamCity log messages
////////////////////////////////////////////////////////////////////

let TeamCityBlock (message : string) action =
    printfn "##teamcity[blockOpened name='%s']" message
    action()
    printfn "##teamcity[blockClosed name='%s']" message

let LocalBlock (message : string) action =
    printfn "\n----------------------------------------"
    printfn "%s" (message.ToUpper())
    printfn "----------------------------------------\n"
    action()

let Block (message : string) action =
    if teamCity = true then (TeamCityBlock message action) else (LocalBlock message action)

////////////////////////////////////////////////////////////////////

Target "Clean" (fun _ ->
    Block "Cleaning directories" (fun _ ->
        CleanDirs [buildDir; binDir; testResultsDir; nugetRoot]
    )
)

Target "Update-TeamCity-Build-Number" (fun _ ->
    Block "Updating TeamCity build number" (fun _ ->
        printfn "##teamcity[buildNumber '%s']" semVersion
    )
)

Target "Set-Versions" (fun _ ->
    Block "Setting version information" (fun _ ->            
        CreateCSharpAssemblyInfo "./src/SolutionInfo.cs"
            [Attribute.Product "Cake"
             Attribute.Version version
             Attribute.FileVersion version
             Attribute.InformationalVersion ((version + buildInfo).Trim())
             Attribute.Copyright "Copyright (c) Patrik Svensson 2014"]
    )
)

Target "Build" (fun _ ->
    Block "Building Cake" (fun _ ->
        MSBuild null "Build" ["Configuration", buildMode] ["./src/Cake.sln"]
        |> Log "AppBuild-Output: "
    )
)

Target "Run-Unit-Tests" (fun _ ->
    Block "Running unit tests" (fun _ ->
        !! (@"./src/**/bin/" + buildMode + "/*.Tests.dll")
            |> xUnit (fun p -> 
                {p with 
                    ShadowCopy = false;
                    HtmlOutput = true;
                    XmlOutput = true;
                    OutputDir = testResultsDir })
    )
)

Target "Copy-Files" (fun _ ->
    Block "Copying files" (fun _ ->
        CopyFile binDir (buildDir + "/Cake.exe")
        CopyFile binDir (buildDir + "/Cake.Core.dll")
        CopyFile binDir (buildDir + "/Cake.Common.dll")
        CopyFile binDir (buildDir + "/NuGet.Core.dll")
        CopyFiles binDir ["LICENSE"; "README.md"; "ReleaseNotes.md"]
    )
)

Target "Create-NuGet-Package" (fun _ ->
    Block "Creating Cake NuGet package" (fun _ ->
        let coreRootDir = nugetRoot @@ "Cake"
        CleanDirs [coreRootDir]

        CopyFile coreRootDir (binDir @@ "Cake.exe")
        CopyFile coreRootDir (binDir @@ "Cake.Core.dll")
        CopyFile coreRootDir (binDir @@ "Cake.Common.dll")
        CopyFile coreRootDir (binDir @@ "NuGet.Core.dll")
        CopyFile coreRootDir (binDir @@ "LICENSE")

        NuGet (fun p ->
            {p with
                Project = "Cake"                           
                OutputPath = nugetRoot
                WorkingDir = coreRootDir
                Version = releaseNotes.AssemblyVersion
                ReleaseNotes = toLines releaseNotes.Notes
                NoPackageAnalysis = true
                AccessKey = getBuildParamOrDefault "nugetkey" ""
                Publish = hasBuildParam "nugetkey" }) "./Cake.nuspec"
    )
)

Target "All" DoNothing

// Setup the target dependency graph.
"Clean"
   =?> ("Update-TeamCity-Build-Number", hasBuildParam "teamCity") 
   ==> "Set-Versions"
   ==> "Build"
   ==> "Run-Unit-Tests"
   ==> "Copy-Files"
   ==> "Create-NuGet-Package"
   ==> "All"

// Set the default target to the last node in the
// target dependency graph.
RunTargetOrDefault "All"