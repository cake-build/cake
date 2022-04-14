### New in 2.2.0 (Released 2022/04/15)

* 3821 PostAction is not setable on DotNetSettings.
* 3485 Add alias for dotnet workload search command.
* 2099 Cache compiled script on disk.
* 3866 Update Microsoft.NETCore.Platforms to 6.0.3.
* 3854 Update Spectre.Console to 0.44.0.
* 3851 Update System.Reflection.Metadata to 6.0.1.
* 3846 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.1.0.
* 3844 Update Microsoft.NETCore.Platforms to 6.0.2.
* 3843 Update NuGet.* to 6.1.0.
* 2763 Provide property to return parent directory on DirectoryPath.
* 2431 UploadFile should support option of username/password.
* 3819 Update Git Release Manager Comment template to remove Cake NuGet package and Chocolatey portable.
* 3859 PathCollapser.Collapse breaks UNC paths.
* 3858 PathCollapser.Collapse shows wrong output for if .. is the second segment in the path.
* 3823 Executing a cake script leads to System.IO.FileNotFoundException for several System.(...) assemblies.
* 3735 Incorrect warnings in diagnostic logs.

### New in 2.1.0 (Released 2022/02/19)

* 2524 XmlTransform support for xsl arguments
* 3479 Add alias for dotnet format command
* 3480 Add alias for dotnet sdk check command
* 3771 Add support for the Chocolatey Export command
* 2746 Add duration of a task
* 3733 Show relative path of addin assemblies that are being loaded
* 3756 Update NuGet.* to 6.0.0
* 3758 Update Autofac to 6.3.0
* 3760 Update Spectre.Console to 0.43.0
* 3764 Add missing GitHub Actions environment info
* 3769 Update Microsoft.NETCore.Platforms to 6.0.1
* 3776 Introduce IPath<T> interface for easier code reuse
* 3777 GitHub Actions UploadArtifact command should accept relative paths
* 3778 Add GitHub Actions DownloadArtifact command
* 3743 SemVersion class crashes if compared to `null`
* 3772 VSTest Alias does not work when only VS 2022 Preview is installed
* 3794 VS2022 BuildTools are not found by the logic introduced in #3775

### New in 2.0.0 (Released 2021/11/30)

* 3714 Use Basic.Reference.Assemblies.* to ensure all standard reference assemblies are available for Roslyn.
* 3654 IsRunningOnAzurePipelines should ignore agent type.
* 3631 Refactor GitHub Actions Paths.
* 3610 Remove TFBuildProvider.
* 3590 Directories in AzurePipelinesBuildInfo are FilePaths - FilePath.GetDirectory then inconsistent.
* 3581 Stop shipping Cake.Portable Chocolatey package and Cake Homebrew formulae.
* 3579 Stop shipping Cake runner for .NET Framework and Cake runner for .NET Core.
* 3577 Remove ReverseDependencyAttribute.
* 3572 Only build for TargetFrameworks netcoreapp3.1, net5.0 and net6.0.
* 3282 GitVersion Tool: Rename verbosity values to match GitVersion values.
* 3222 Add Xamarin.iOS platform targets to MSBuildSettings PlatformTarget enumeration.
* 3151 Add support for Engine event hooks after execution as well as before.
* 3003 Remove DependencyAttribute.
* 2872 Bump eol target frameworks.
* 2788 Tool:OpenCover - the register-setting should be an option, rather than a string.
* 1111 DotNetCoreRestore: dotnet restore no longer supports globbing.
* 3630 Add GitHub Actions Environment properties.
* 3629 Add GitHub Actions UploadArtifact Command.
* 3628 Add GitHub Actions SetEnvironmentVariable Command.
* 3627 Add GitHub Actions AddPath Command.
* 3341 Epic: Introduce DotNet aliases (synonyms to DotNetCore aliases).
* 3709 Arguments alias should support ICollection as default value.
* 3691 Update Microsoft.NETCore.Platforms to 6.0.0.
* 3690 Update Microsoft.Extensions.DependencyInjection to 6.0.0.
* 3689 Update System.Reflection.Metadata to 6.0.0.
* 3688 Update System.Collections.Immutable to 6.0.0.
* 3681 ScriptAssemblyResolver logging should be at debug/diagnostic level.
* 3662 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-6.final.
* 3647 Display message of criteria when task fails to run due to criteria not being met.
* 3644 Add DotNetNuGetUpdateSource aliases (synonym to DotNetCoreNuGetUpdateSource).
* 3643 Add DotNetNuGetRemoveSource aliases (synonym to DotNetCoreNuGetRemoveSource).
* 3642 Add DotNetNuGetListSourceSettings (derived from to DotNetNuGetSource).
* 3641 Add DotNetNuGetHasSource aliases (synonym to DotNetCoreNuGetHasSource).
* 3640 Add DotNetNuGetEnableSource aliases (synonym to DotNetCoreNuGetEnableSource).
* 3639 Add DotNetNuGetDisableSource aliases (synonym to DotNetCoreNuGetDisableSource).
* 3607 Add EnableCompressionInSingleFile to DotNetCorePublishSettings.
* 3599 Add VS2022 to default MSBuild Resolver.
* 3598 Remove Preview from VS2022 MSBuild Resolver.
* 3595 Update Autofac to 6.3.0.
* 3593 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-5.final.
* 3591 Update Microsoft.NETCore.Platforms to 6.0.0-rc.2.21480.5.
* 3555 Add DotNetNuGetAddSource aliases (synonym to DotNetCoreNuGetAddSource).
* 3554 Add DotNetNuGetDelete aliases (synonym to DotNetCoreNuGetDelete).
* 3553 Add DotNetNuGetPush aliases (synonym to DotNetCoreNuGetPush).
* 3552 Add DotNetPack alias (synonym to DotNetCorePack).
* 3551 Add DotNetPublish alias (synonym to DotNetCorePublish).
* 3550 Add DotNetVSTest alias (synonym to DotNetCoreVSTest).
* 3549 Add DotNetTest alias (synonym to DotNetCoreTest).
* 3548 Add DotNetBuildServer alias (synonym to DotNetCoreBuildServer).
* 3547 Add DotNetBuild alias (synonym to DotNetCoreBuild).
* 3546 Add DotNetRestore alias (synonym to DotNetCoreRestore).
* 3545 Add DotNetClean alias (synonym to DotNetCoreClean).
* 3544 Add DotNetExecute alias (synonym to DotNetCoreExecute).
* 3543 Add DotNetRun alias (synonym to DotNetCoreRun).
* 3542 Add DotNetTool alias (synonym to DotNetCoreTool).
* 3523 Add DotNetMSBuild alias (synonym to DotNetCoreMSBuild).
* 3215 Add RunCommand with postAction parameter to DotNetCoreTool.
* 3075 Make FilePath and DirectoryPath comparable by value.
* 2571 OctopusDeploy DeployTo property to take collection of string to specify multiple environments.
* 2075 Add overloads for DotNetCore*() methods taking FilePath instead of string.
* 1794 Private is missing from ProjectReference.
* 1616 Error message on circular references leads to poor developer experience.
* 3701 Add cake-module tag to Cake.DotNetTool.Module NuGet package.
* 3602 Switch to Cake.Tool as primary package in REAME.md.
* 3711 SemanticVersion missing equals/not equals operator, prerelease sorted wrong.
* 3697 Error: The requested service 'Cake.Commands.DefaultCommandSettings' has not been registered.
* 3693 Core suffix is still used in some settings classes.
* 3683 Use DotNetMSBuildSettings instead of DotNetCoreMSBuildSettings on new dotnet aliases settings.
* 3671 VS2022: msbuild can not be located, only Build Tools are installed.
* 2665 C*  8 Using Statement produces compile error.
* 2443 Erroneous "Target path must be an absolute path" when preserveFolderStructure is used with CopyFiles.
* 1669 Release notes does not tolerate prerelease versions.

### New in 2.0.0-rc0002 (Released 2021/11/26)

* 3714 Use Basic.Reference.Assemblies.* to ensure all standard reference assemblies are available for Roslyn
* 3654 IsRunningOnAzurePipelines should ignore agent type
* 3631 Refactor GitHub Actions Paths
* 3610 Remove TFBuildProvider
* 3590 Directories in AzurePipelinesBuildInfo are FilePaths - FilePath.GetDirectory then inconsistent
* 3581 Stop shipping Cake.Portable Chocolatey package and Cake Homebrew formulae
* 3579 Stop shipping Cake runner for .NET Framework and Cake runner for .NET Core
* 3577 Remove ReverseDependencyAttribute
* 3572 Only build for TargetFrameworks netcoreapp3.1, net5.0 and net6.0
* 3282 GitVersion Tool: Rename verbosity values to match GitVersion values
* 3222 Add Xamarin.iOS platform targets to MSBuildSettings PlatformTarget enumeration
* 3151 Add support for Engine event hooks after execution as well as before
* 3003 Remove DependencyAttribute
* 2872 Bump eol target frameworks
* 2788 Tool:OpenCover - the register-setting should be an option, rather than a string
* 1111 DotNetCoreRestore: dotnet restore no longer supports globbing
* 3630 Add GitHub Actions Environment properties
* 3629 Add GitHub Actions UploadArtifact Command
* 3628 Add GitHub Actions SetEnvironmentVariable Command
* 3627 Add GitHub Actions AddPath Command
* 3341 Epic: Introduce DotNet aliases (synonyms to DotNetCore aliases)
* 3711 SemanticVersion missing equals/not equals operator, prerelease sorted wrong
* 3697 Error: The requested service 'Cake.Commands.DefaultCommandSettings' has not been registered
* 3693 `Core` suffix is still used in some settings classes
* 3683 Use DotNetMSBuildSettings instead of DotNetCoreMSBuildSettings on new dotnet aliases settings
* 3671 VS2022: msbuild can not be located, only Build Tools are installed
* 2443 Erroneous "Target path must be an absolute path" when preserveFolderStructure is used with CopyFiles
* 1669 Release notes does not tolerate prerelease versions
* 3709 Arguments alias should support ICollection<T> as default value
* 3691 Update Microsoft.NETCore.Platforms to 6.0.0
* 3690 Update Microsoft.Extensions.DependencyInjection to 6.0.0
* 3689 Update System.Reflection.Metadata to 6.0.0
* 3688 Update System.Collections.Immutable to 6.0.0
* 3681 `ScriptAssemblyResolver` logging should be at debug/diagnostic level
* 3662 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-6.final
* 3647 Display message of criteria when task fails to run due to criteria not being met
* 3644 Add DotNetNuGetUpdateSource aliases (synonym to DotNetCoreNuGetUpdateSource)
* 3643 Add DotNetNuGetRemoveSource aliases (synonym to DotNetCoreNuGetRemoveSource)
* 3642 Add DotNetNuGetListSourceSettings (derived from to DotNetNuGetSource)
* 3641 Add DotNetNuGetHasSource aliases (synonym to DotNetCoreNuGetHasSource)
* 3640 Add DotNetNuGetEnableSource aliases (synonym to DotNetCoreNuGetEnableSource)
* 3639 Add DotNetNuGetDisableSource aliases (synonym to DotNetCoreNuGetDisableSource)
* 3607 Add `EnableCompressionInSingleFile` to `DotNetCorePublishSettings`
* 3599 Add VS2022 to default MSBuild Resolver
* 3598 Remove Preview from VS2022 MSBuild Resolver
* 3595 Update Autofac to 6.3.0
* 3593 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-5.final
* 3591 Update Microsoft.NETCore.Platforms to 6.0.0-rc.2.21480.5
* 3555 Add DotNetNuGetAddSource aliases (synonym to DotNetCoreNuGetAddSource)
* 3554 Add DotNetNuGetDelete aliases (synonym to DotNetCoreNuGetDelete)
* 3553 Add DotNetNuGetPush aliases (synonym to DotNetCoreNuGetPush)
* 3552 Add DotNetPack alias (synonym to DotNetCorePack)
* 3551 Add DotNetPublish alias (synonym to DotNetCorePublish)
* 3550 Add DotNetVSTest alias (synonym to DotNetCoreVSTest)
* 3549 Add DotNetTest alias (synonym to DotNetCoreTest)
* 3548 Add DotNetBuildServer alias (synonym to DotNetCoreBuildServer)
* 3547 Add DotNetBuild alias (synonym to DotNetCoreBuild)
* 3546 Add DotNetRestore alias (synonym to DotNetCoreRestore)
* 3545 Add DotNetClean alias (synonym to DotNetCoreClean)
* 3544 Add DotNetExecute alias (synonym to DotNetCoreExecute)
* 3543 Add DotNetRun alias (synonym to DotNetCoreRun)
* 3542 Add DotNetTool alias (synonym to DotNetCoreTool)
* 3523 Add DotNetMSBuild alias (synonym to DotNetCoreMSBuild)
* 3215 Add RunCommand with postAction parameter to DotNetCoreTool
* 3075 Make FilePath and DirectoryPath comparable by value
* 2571 OctopusDeploy DeployTo property to take collection of string to specify multiple environments
* 2075 Add overloads for DotNetCore*() methods taking FilePath instead of string
* 1794 Private is missing from ProjectReference
* 1616 Error message on circular references leads to poor developer experience
* 3701 Add cake-module tag to Cake.DotNetTool.Module NuGet package
* 3602 Switch to Cake.Tool as primary package in REAME.md

### New in 2.0.0-rc0001 (Released 2021/11/07)

* 3654 IsRunningOnAzurePipelines should ignore agent type
* 3631 Refactor GitHub Actions Paths
* 3610 Remove TFBuildProvider
* 3590 Directories in AzurePipelinesBuildInfo are FilePaths - FilePath.GetDirectory then inconsistent
* 3581 Stop shipping Cake.Portable Chocolatey package and Cake Homebrew formulae
* 3579 Stop shipping Cake runner for .NET Framework and Cake runner for .NET Core
* 3577 Remove ReverseDependencyAttribute
* 3572 Only build for TargetFrameworks netcoreapp3.1, net5.0 and net6.0
* 3282 GitVersion Tool: Rename verbosity values to match GitVersion values
* 3222 Add Xamarin.iOS platform targets to MSBuildSettings PlatformTarget enumeration
* 3151 Add support for Engine event hooks after execution as well as before
* 3003 Remove DependencyAttribute
* 2872 Bump eol target frameworks
* 2788 Tool:OpenCover - the register-setting should be an option, rather than a string
* 1111 DotNetCoreRestore: dotnet restore no longer supports globbing
* 3341 Introduce DotNet aliases (synonyms to DotNetCore aliases
* 3627 Add GitHub Actions AddPath Command
* 3628 Add GitHub Actions SetEnvironmentVariable Command
* 3629 Add GitHub Actions UploadArtifact Command
* 3630 Add GitHub Actions Environment properties
* 3662 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-6.final
* 3647 Display message of criteria when task fails to run due to criteria not being met
* 3644 Add DotNetNuGetUpdateSource aliases (synonym to DotNetCoreNuGetUpdateSource)
* 3643 Add DotNetNuGetRemoveSource aliases (synonym to DotNetCoreNuGetRemoveSource)
* 3642 Add DotNetNuGetListSourceSettings (derived from to DotNetNuGetSource)
* 3641 Add DotNetNuGetHasSource aliases (synonym to DotNetCoreNuGetHasSource)
* 3640 Add DotNetNuGetEnableSource aliases (synonym to DotNetCoreNuGetEnableSource)
* 3639 Add DotNetNuGetDisableSource aliases (synonym to DotNetCoreNuGetDisableSource)
* 3607 Add EnableCompressionInSingleFile to DotNetCorePublishSettings
* 3599 Add VS2022 to default MSBuild Resolver
* 3598 Remove Preview from VS2022 MSBuild Resolver
* 3595 Update Autofac to 6.3.0
* 3593 Update Microsoft.CodeAnalysis.CSharp.Scripting to 4.0.0-5.final
* 3591 Update Microsoft.NETCore.Platforms to 6.0.0-rc.2.21480.5
* 3555 Add DotNetNuGetAddSource aliases (synonym to DotNetCoreNuGetAddSource)
* 3554 Add DotNetNuGetDelete aliases (synonym to DotNetCoreNuGetDelete)
* 3553 Add DotNetNuGetPush aliases (synonym to DotNetCoreNuGetPush)
* 3552 Add DotNetPack alias (synonym to DotNetCorePack)
* 3551 Add DotNetPublish alias (synonym to DotNetCorePublish)
* 3550 Add DotNetVSTest alias (synonym to DotNetCoreVSTest)
* 3549 Add DotNetTest alias (synonym to DotNetCoreTest)
* 3548 Add DotNetBuildServer alias (synonym to DotNetCoreBuildServer)
* 3547 Add DotNetBuild alias (synonym to DotNetCoreBuild)
* 3546 Add DotNetRestore alias (synonym to DotNetCoreRestore)
* 3545 Add DotNetClean alias (synonym to DotNetCoreClean)
* 3544 Add DotNetExecute alias (synonym to DotNetCoreExecute)
* 3543 Add DotNetRun alias (synonym to DotNetCoreRun)
* 3542 Add DotNetTool alias (synonym to DotNetCoreTool)
* 3523 Add DotNetMSBuild alias (synonym to DotNetCoreMSBuild)
* 3215 Add RunCommand with postAction parameter to DotNetCoreTool
* 3075 Make FilePath and DirectoryPath comparable by value
* 2571 OctopusDeploy DeployTo property to take collection of string to specify multiple environments
* 2075 Add overloads for DotNetCore*() methods taking FilePath instead of string
* 1794 Private is missing from ProjectReference
* 1616 Error message on circular references leads to poor developer experience
* 1669 Release notes does not tolerate prerelease versions
* 2443 Erroneous "Target path must be an absolute path" when preserveFolderStructure is used with CopyFiles
* 3602 Switch to Cake.Tool as primary package in REAME.md

### New in 1.3.0 (Released 2021/10/07)

* 3469 Add support for .NET 6
* 3493 .NET CLI Build Binary log filenames aren't quoted correctly
* 3477 parsing of solution files with absolute paths to  projects throws exception
* 3455 NuGet Resolver native dependencies fails on latest macOS
* 3352 Cake Frosting Parent DirectoryPath Fails To Combine with Slash
* 3291 Unable to retrieve target argument with Frosting
* 2048 DotNetCoreToolSettings.WorkingDirectory is not respected when running DotNetCoreTool
* 3521 Update Microsoft.NETCore.Platforms to 6.0.0-rc.1.21451.13
* 3519 Update Spectre.Console to 0.42.0
* 3503 Add NuGet Sources argument to DotNetCoreTestSettings
* 3502 Add NuGet Sources argument to DotNetCoreRunSettings
* 3501 Add NuGet Sources argument to DotNetCorePackSettings
* 3464 Support MSBuild version 17
* 3452 Missing option in InspectCodeSettings: `--build` and `--no-build` flags
* 3449 Add Version, AssemblyVersion, FileVersion, and AssemblyInformationalVersion properties to DotNetCoreMSBuildSettings
* 3447 Add ContinuousIntegrationBuild to DotNetCoreMSBuildSettings
* 3445 Highlight failed tasks on summary when Error handler is defined
* 3237 Allow setting MSBuildToolVersion using custom string - Part 1
* 3065 Add DOTNET_ROLL_FORWARD setting to DotNetCoreSettings
* 2165 DotNetCore Build misses Sources settings
* 2104 Make possibility to set Process Exit Code
* 1882 DeleteDirectory throws exception if directory doesn't exist
* 3515 Add a simple README to the packages to be shown on NuGet.org
* 3466 Fix two typos in GitReleaseManagerAliases documentation

### New in 1.2.0 (Released 2021/08/29)

* 2690 Consider adding some kind of "GetArguments()" alias, similar to the EnvironmentVariables() one.
* 2578 Feature request: nuget version ranges support.
* 2362 Add Support for New snupkg Symbol Packages.
* 3429 Microsoft.Extensions.DependencyInjection to 5.0.2.
* 3427 Update Microsoft.CodeAnalysis.CSharp.Scripting to 3.11.0.
* 3425 Update NuGet Client libraries to 5.11.0.
* 3423 Update Spectre.Console to 0.41.0.
* 3337 Suppress compilation warnings CS1701, CS1702, and CS1705.
* 3316 Bump NuGet client libraries to 5.9.1.
* 3314 Bump .NET SDK to 5.0.202.
* 3294 Clean up task builder extensions.
* 3281 GitVersion Tool: Remap existing verbosity values to valid GitVersion values.
* 3255 Update NuGet client libraries to 5.9.0.
* 3253 Update Microsoft.CodeAnalysis.CSharp.Scripting to 3.9.0 stable.
* 3246 Update Spectre.Console to 0.38.0.
* 3223 Feature request: Environment variable substitution in cake.config.
* 2654 NUnit3Settings should support TestParam.
* 2168 TypeConverter to enable Argument(...).
* 2030 NuGetHasSource is case sensitive.
* 3365 Typo in documentation of NuGetAdd alias.
* 3355 VSTest alias documentation contains holdover from <v0.17.0.
* 3283 Update GitVersion alias reference page with dotnet tool usage example.
* 3259 TypeLoadException: Missing implementation of RegisterLazy.
* 3431 Update Microsoft.NET.Test.Sdk to 16.11.0.
* 3421 Update .NET SDK to 5.0.400.
* 3372 Update Microsoft.NET.Test.Sdk to 16.9.4.
* 3370 Update Spectre.Console to 0.39.0.
* 3368 Update .NET SDK to 5.0.203 and .NET Core 3.1.409 and 2.1.816.
* 3250 Update .NET SDK to 5.0.200.
* 3248 Remove Cake.DotNetTool.Module from build.cake.
* 3360 using Spectre.Console; makes error CS0246.
* 3352 Cake Frosting Parent DirectoryPath Fails To Combine with Slash.
* 3243 Error messages logged via Error(...) are displayed in random places in the build log.
* 3226 Still can't resolve resource assemblies after GH2734.
* 1663 CopyFiles alias throws exception if empty enumeration is passed.

### New in 1.1.0 (Released 2021/03/06)

* 2983 No possibility of adding variable with isOutput=true in Azure Pipelines
* 2903 Integrate Cake.DotNetTool.Module
* 2685 Better dotnet tool integration
* 3190 Working directory is not respected in BuildContext constructor
* 3143 ParseSolution throws IndexOutOfRangeException on empty lines
* 3058 Regression: Tools are no longer not restored in working directory
* 2852 Terminal output colours
* 3219 Update nuspec iconUrl in packages to use CDN URL
* 3216 Remap NuGetLogger Verbose/Verbose to ICakeLog Debug/Diagnostic
* 3213 Add iconUrl fallback to Cake and Cake.CoreCLR packages
* 3193 Update Cake unit MS Test SDK dependencies to 16.9.1
* 3191 Update Roslyn (Microsoft.CodeAnalysis.CSharp.Scripting) to 3.9.0-4.final
* 3188 Update Cake.NuGet dependencies to latest stable
* 3144 TeamCity pull request info requires "GIT_BRANCH" environment variable
* 3133 Add IncludeNativeLibrariesForSelfExtract and IncludeAllContentForSelfExtract to DotNetCorePublishSettings
* 3127 Add ResultsDirectory to VSTestSettings
* 3125 ReportGenerator missing report types
* 3081 Add netcoreapp3.1 target to Cake
* 3066 Prefer tools with platform affinity
* 3040 Package ID Prefix Reservation for Cake.* on nuget.org for cake-build organization
* 3024 WindowsRegistry: Expose other root registry keys to Cake scripts (e.g. HKEY_CURRENT_USER)
* 2975 Add support for opting out of ANSI coloring via NO_COLOR env. variable
* 2967 Expose TeamCity build properties dictionary via TeamCityBuildInfo
* 2966 Enable AnsiConsoleRenderer in TeamCity and Azure Pipelines
* 2955 Add Build Start Date & Time to TeamCity build information
* 2941 ProcessArgumentBuilder helpers should return empty builder when values is null
* 2932 Add dotnet test --blame Flag to DotNetCoreTestSettings
* 2314 There's no way to set platform like Debug|iPhone
* 1633 Some command line output ignores system foreground color configuration
* 2904 (Frosting) Tool installer should respect configuration

### New in 1.0.0 (Released 2021/02/07)

* 3050 Frosting: Rename CakeHost extension from UseTool to InstallTool.
* 2930 Increase potential breaking change property.
* 2333 RFC-0001: Rewrite Cake CLI.
* 2292 Remove obsolete methods and properties.
* 3020 Migrate to Spectre.Console.
* 2933 Enable NuGet provider in Frosting.
* 2874 Merge frosting into main Cake repo.
* 2883 (Frosting) Add support for .NET 5.
* 2857 Add support for .NET 5.
* 2776 Checklist for 1.0.
* 2755 Add DirectoryHashCalculator.
* 2199 Add GlobPattern class.
* 741 Add IsRunningOnMacOs() alias.
* 3083 Update --tree usage example to match option in the help info.
* 3069 Don't promote UseWorkingDirectory in Frosting default template.
* 3029 Add ICakeArguments.GetArgument extension.
* 3018 Cake displays raw ANSI output after running specific executables.
* 3009 Make Cake Core CakeDataService Public.
* 2913 Add overload for DotNetCoreRun.
* 2908 Future proof .NET [Core] detection.
* 2897 Add tests for MyGetProvider.
* 2895 Custom contexts should inherit from CakeContextAdapter.
* 2877 Add NuGet's Icon setting to NuGetPackSettings.
* 2870 Add helpers for adding multiple strings to ProcessArgumentBuilder.
* 2866 Support multiple dotnet test options.
* 2847 Add new GitHub Actions URL environment variables.
* 2844 Add missing dotnet test options.
* 2839 Add support for PublishReadyToRunShowWarnings property in DotNetCorePublish.
* 2838 Add MakeRelative alias to DirectoryPath and FilePath.
* 2833 Implicit bootstrapping of modules.
* 2831 ParseAssemblyInfo does not detect lines with extra spaces.
* 2886 (Frosting) Support all commands that Cake does.
* 2825 Add option to ignore tool exit code.
* 2822 Add support of ReportGenerator global tool.
* 2820 Add DebuggerStepThroughAttribute to generated code.
* 2817 Bump dependencies.
* 2801 Inconsistent NuGet file name case.
* 2792 Add dotnet nologo options.
* 2743 Tool resolution for multiple names should be breadth first.
* 2703 OpenCover is missing hideskipped setting.
* 2623 DotNetCoreTestSettings Can Have Multiple Logger's.
* 2595 Misleading output message when trying to install prerelease package with the in-process nuget installer.
* 2892 (Frosting) Add ANSI console.
* 2893 (Frosting) Align command line parsing with Cake.
* 2962 Document breaking changes in 1.0 CLI.
* 2925 Fix sentences which end with double full stop.
* 2918 Incorrect link for ReSharper's Open Source webpage in Cake readme.
* 2894 Remove unnecessary documentation and replace it with .
* 2879 Update links pointing to cakebuild.net to new URL structure.
* 2836 Update README with more up-to-date "getting started" information.
* 2811 Identity of BuildProblem in TeamCityProvider should be optional.
* 1690 Casing causes 'More than one build script specified.' message.
* 3077 Regression: rc0003 outputs extra characters on OSX.
* 3072 Attribute [IsDependeeOf] doesn't work.
* 3038 Tool resolving in Frosting tasks.
* 3032 Frosting project fails on Linux.
* 3007 Different arguments between script runner and Frosting.
* 2963 EndOfStreamException thrown when using loaddependencies=true.
* 2961 Update dotnet cake usage instructions (dotnet cake --help).
* 2956 Wrong Cake version in build.config.
* 2911 C*  syntax errors in exceptions causes Specre.CLI internal error.
* 2861 Fix error output in 1.0 preview.
* 2853 Custom argument names are not case insensitive in 1.0 preview.
* 2887 (Frosting) Fix line endings in build.sh within template package.
* 2734 Can't resolve resource assemblies.
* 2066 cake.coreclr help information error.

### New in 1.0.0-rc0003 (Released 2021/01/29)

* 3029 Add ICakeArguments.GetArgument extension
* 3007 Different arguments between script runner and Frosting
* 3018 Cake displays raw ANSI output after running specific executables
* 3009 Make Cake Core CakeDataService Public
* 2961 Update dotnet cake usage instructions (dotnet cake --help)
* 2066 cake.coreclr help information error
* 3032 Frosting project fails on Linux
* 3050 Frosting: Rename CakeHost extension from UseTool to InstallTool
* 3020 Migrate to Spectre.Console

### New in 1.0.0-rc0002 (Released 2020/12/20)

* 2930 Increase potential breaking change property.
* 2904 (Frosting) Tool installer should respect configuration.
* 2933 Enable NuGet provider in Frosting.
* 2838 Add MakeRelative alias to DirectoryPath and FilePath.
* 2886 (Frosting) Support all commands that Cake does.
* 2893 (Frosting) Align command line parsing with Cake.
* 2892 (Frosting) Add ANSI console.
* 2962 Document breaking changes in 1.0 CLI.
* 2980 Update to .NET 5 SDK 5.0.101.
* 2929 GitReleaseManager milestone should use SemVersion.
* 2928 Cake.Frosting and Cake.Frosting.Template not pushed to NuGet.
* 2900 Update to .NET 5 SDK "RTM".
* 2963 EndOfStreamException thrown when using loaddependencies=true.
* 2956 Wrong Cake version in build.config.

### New in 1.0.0-rc0001 (Released 2020/11/05)

* 2292 Remove obsolete methods and properties.
* 2874 Merge frosting into main Cake repo.
* 2883 (Frosting) Add support for .NET 5.
* 2857 Add support for .NET 5.
* 2776 Checklist for 1.0.
* 2755 Add DirectoryHashCalculator.
* 2333 RFC-0001: Rewrite Cake CLI.
* 2199 Add GlobPattern class.
* 741 Add IsRunningOnMacOs() alias.
* 2913 Add overload for DotNetCoreRun.
* 2908 Future proof .NET [Core] detection.
* 2897 Add tests for MyGetProvider.
* 2895 Custom contexts should inherit from CakeContextAdapter.
* 2877 Add NuGet's Icon setting to NuGetPackSettings.
* 2870 Add helpers for adding multiple strings to ProcessArgumentBuilder.
* 2866 Support multiple dotnet test options.
* 2847 Add new GitHub Actions URL environment variables.
* 2844 Add missing dotnet test options.
* 2839 Add support for PublishReadyToRunShowWarnings property in DotNetCorePublish.
* 2833 Implicit bootstrapping of modules.
* 2831 ParseAssemblyInfo does not detect lines with extra spaces.
* 2825 Add option to ignore tool exit code.
* 2822 Add support of ReportGenerator global tool.
* 2820 Add DebuggerStepThroughAttribute to generated code.
* 2817 Bump dependencies.
* 2801 Inconsistent NuGet file name case.
* 2792 Add dotnet nologo options.
* 2743 Tool resolution for multiple names should be breadth first.
* 2703 OpenCover is missing hideskipped setting.
* 2623 DotNetCoreTestSettings Can Have Multiple Logger's.
* 2595 Misleading output message when trying to install prerelease package with the in-process nuget installer.
* 2925 Fix sentences which end with double full stop.
* 2918 Incorrect link for ReSharper's Open Source webpage in Cake readme.
* 2894 Remove unnecessary documentation and replace it with .
* 2879 Update links pointing to cakebuild.net to new URL structure.
* 2836 Update README with more up-to-date "getting started" information.
* 2811 Identity of BuildProblem in TeamCityProvider should be optional.
* 2920 Bump Cake script dependencies.
* 2899 Update to .NET 5 SDK RC 2.
* 2850 Bump .NET Core SDK to 3.1.402.
* 2818 Start producing 1.0.0-rc0001 NuGet Packages.
* 2814 Switch GRM to not mark GitHub release as a pre-release.
* 2781 Bump StyleCop to latest version.
* 2911 C# syntax errors in exceptions causes Specre.CLI internal error.
* 2861 Fix error output in 1.0 preview.
* 2853 Custom argument names are not case insensitive in 1.0 preview.
* 2887 (Frosting) Fix line endings in build.sh within template package.
* 2734 Can't resolve resource assemblies.

### New in 0.38.5 (Released 2020/09/20)

* 2859 .NET 5 shouldn't be identified as Full Framework or Mono.

### New in 0.38.4 (Released 2020/06/26)

* 2813 Actually ship  0.38.3

### New in 0.38.3 (Released 2020/06/26)

* 2803 ArgumentOutOfRangeException: The DateTimeOffset specified cannot be converted into a Zip file timestamp .
* 2798 Input string not in correct format starting in v0.38.0.
* 2799 Bump .NET Core SDK to 3.1.301.

### New in 0.38.2 (Released 2020/06/09)

* 2790 Cake 0.38.1 is failing to write messages to TeamCity

### New in 0.38.1 (Released 2020/05/30)

* 2786 0.38.0 introduces TFBuild obsolete warning for everyone not just users of property

### New in 0.38.0 (Released 2020/05/30)

* 2784 Add NuGet Delete functionality
* 2749 Add support for ANSI escape codes
* 2728 Add dotnet NuGet source commands
* 2718 Add binary logger to dotnet MSBuild settings
* 2721 NuGet package name/path should be added in quotes
* 2785 Provide additional logging for tool resolver
* 2778 Update Roslyn to 3.6.0
* 2768 Add an option to skip the default warning/error output when running JetBrains command line tools
* 2764 Additional Jenkins information
* 2752 MSTestRunner prioritizes VS2017 over VS2019
* 2745 Extend XmlPeekSettings with FileShare.* option
* 2733 NuGetSetApiKey with Verbosity set to Quiet causes an exception.
* 2730 Better handling for GitVersion failure cases
* 2715 Add missing dotnet NuGet command options
* 2714 Squash warning when skip package version check
* 2710 Add GitHub Actions GITHUB_RUN_ID & GITHUB_RUN_NUMBER
* 2658 Rename TFBuild alias to AzurePipelines
* 2077 Locating the correct vstest.console.exe (VS2017)
* 2735 Improve documentation for MSBuild alias to make clear that also MSBuild projects can be passed

### New in 0.37.0 (Released 2020/02/01)

* 2708 Emitting debug information should be done regardless of --debug switch.
* 2701 Api keys should be secret.
* 2697 Update to latest release of GitReleaseManager.
* 2696 Upgrade Cake Alias support for GitReleaseManager.
* 2691 Missing GitHubActions Alias.
* 2695 Cake fails to build on travis-ci with latest mono (6.8).
* 2693 Update .NET Core SDK to 3.1.101.

### New in 0.36.0 (Released 2020/01/11)

* 2677 Add GitHub Actions build provider.
* 2638 CreateAssemblyInfo alias should allow creation of custom boolean attributes and also empty attributes.
* 2682 Add .NET Core 3.1 Runtime support.
* 2679 Add optional GetToolExecutableNames that takes tool settings.
* 2675 InspectCode: Add support for InspectCode.x86.exe tool.
* 2673 DotNetCorePublishSettings should support new .NET Core 3 features.
* 2671 NuGet Install doesn't allow to set NonInteractive to false.
* 2663 Support Inno Setup 6.
* 2657 Rename TFBuild.Environment.Repository.Branch to TFBuild.Environment.Repository.BranchName.
* 2533 Setting SecurityRules with CreateAssemblyInfo.
* 2532 Publish Cake.Testing.Xunit package.
* 2328 InspectCode: Add support for new /verbosity argument.
* 2652 Improve documentation for TFBuildPullRequestInfo.Id and TFBuildPullRequestInfo.Number.
* 2661 Build not stopped when rethrowing exception in OnError.
* 2640 Remove unnecessary parameter from AddMetadataAttribute method.
* 2637 CreateAssemblyInfo alias creates invalid file when using .vb.
* 2534 Windows 10: System.PlatformNotSupportedException: System.Data.SqlClient is not supported on this platform.
* 2527 Don't limit the #load to only .cake files.
* 2498 ParseProject fails on an absolute HintPath.
* 2275 System.Data.SqlClient in Cake.CoreCLR Assembly Load Error.

### New in 0.35.0 (Released 2019/09/28)

* 2603 Add .NET Core 3 to Cake.Tool update to .NET Core 3 SDK.
* 2625 Add NuGet Push -SkipDuplicate Flag.
* 2618 The MSTest tool doesn't pick up the mstest.exe from Visual Studio 2019.
* 2606 Unable to reference Newtonsoft.Json > 11.0.2.
* 2601 Update Microsoft.CodeAnalysis.CSharp.Scripting to 3.2.1.
* 2599 Update to Autofac 4.9.4.
* 2585 Cake.Tool - How in the world do I run a specific task?.
* 2590 Update confusing GitVersionVerbosity docs.
* 2610 Aliases of type 'dynamic' cannot be accessed directly.
* 2608 TFBuildProvider.IsHostedAgent returns wrong value when running on 2nd build agent.

### New in 0.34.1 (Released 2019/07/16)

* 2575 v0.34.0 fails on scripts using the dynamic keyword

### New in 0.34.0 (Released 2019/07/16)

* 2519 Not able to build project with ToolsVersion="15.0"
* 2553 cake 0.33.0 compilation is failing for System.Net.Http.HttpClient on Mono 5.20.1.19
* 2535 OctoPack doesn't work on Linux
* 2161 If [Nuget] ConfigFile directive in cake configuration file has no folder — error rises
* 2157 NuGetPack with nuspec that contains contentFiles becomes invalid
* 2560 Runtime property is missing for 'dotnet pack', 'dotnet run' and 'dotnet clean'
* 2556 DotNetCoreTestSettings: Missing RunTime Property which is needed for RID builds
* 2551 Call MSBuild without specifying a target does not use DefaultTarget
* 2536 Additional formatting options on XmlPoke
* 2531 Update to NuGet client libraries to v5
* 2530 Remove dependency on NuGet.PackageManagement
* 2521 Update to Roslyn 3.0.0
* 2499 NuGet Pack with assembly references support
* 2156 Add newer nuspec properties to NuGetPackSettings
* 1618 Support different Git servers in TeamCityPullRequestInfo

### New in 0.33.0 (Released 2019/04/01)

* 2514 Add additional report types for ReportGenerator
* 2130 Add exceptions thrown to TaskTeardownContext
* 2456 Add logging aliases to override the log verbosity
* 2453 Unify pull request status across providers
* 2440 Add EnvironmentVariable alias
* 2400 Add globber pattern support to the #load directive
* 2504 Update .NET Core SDK 2.1.505
* 2487 Warn and skip code gen for duplicate aliases
* 2481 FilePath and DirectoryPath implicit conversions should return null when passed null
* 2473 ParseAssemblyInfo does not support .NET Core generated assembly info
* 2468 DotNet commands do not respect the verbosity
* 2439 HtmlInline_AzurePipelines and MHtml shares the same numeric value
* 2432 Azure Pipelines build system not recognized with non-Windows jobs
* 2088 VSWhere -requires and -products argument values are quoted but VSWhere doesn't support multiple values in quotes
* 2507 Cake.CoreCLR can't handle whitespace in path
* 2491 Add additional Azure DevOps (TFBuild) properties
* 2484 Octopus Deploy 2019.1 and Spaces feature
* 2478 Lock file arguments for NuGet and dotnet restore
* 2474 TeamCityProvider.BuildProblem method should conform to TeamCity API
* 2472 Expose ICakeConfiguration (or specific values like tools path) on context
* 2465 Roundhouse dotnet tool does not run
* 2463 DoesForEach don't support data context for items functions
* 2462 Added unit tests for Cake.Core
* 2459 Add MSBuildPath to NuGetRestoreSettings
* 2449 ARM64 missing from MSBuild target platform
* 2445 Add OnError
* 2433 NugetRestore still using msbuild 15
* 2429 Add provider name to BuildSystem
* 2415 Add support for MSBuild options to enable RestoreLockedMode
* 2393 MethodAliasGenerator doesn't generate parameter attributes
* 2345 Allow NuGetRestoreSettings to opt out of setting -NonInteractive
* 2270 Allow to listen and modify redirected standard output of a process
* 2141 Add Verbosity property to GitVersionSettings
* 2124 Add Support for IEnumerable tokens on TextTransformationExtensions
* 2087 Include more detailed exception information when Exception is AggregateException
* 2026 Support for additional SignTool flags
* 2019 Clean up some parser tests
* 1384 Enhancement: Add support for filtering files in Globbing alias
* 820 Log tools command-line at higher log level (preferably default)
* 2512 TFBuildPublishCodeCoverageData xml comments minor typo
* 2025 The tool path for MSpec needs to be changed in the documentation

### New in 0.32.1 (Released 2019/01/04)

* 2426 Chocolatey pack regression in Cake 0.32.0

### New in 0.32.0 (Released 2019/01/04)

* 2420 Add new label alias for GitReleaseManager
* 2419 Extend GitReleaseManager aliases to use token parameter
* 2424 Support computer cert store with SignTool
* 2417 Extend GetToolExecutableNames for GitReleaseManager
* 2412 TFBuildCommand PublishCodeCoverage API Changes
* 2410 Add Global Tool and new arguments support in TextTemplatingAliases
* 2398 Support MsBuild version (16)
* 2381 Zip should behave by default like standard Zip utilities
* 2379 Add an Encoding parameter to TextTransformation.Save
* 2327 Missing report types for ReportGenerator
* 2294 Add fluent API to enable MSBuild binary logger
* 2249 Unhelpful error when * loading a missing nuget package
* 2243 Missing ResultsDirectory when using DotNetCoreVSTest
* 1973 Add Products prop to VSWhereSettings
* 2397 Fix missing parenthesis and missing setting.

### New in 0.31.0 (Released 2018/12/13)

* 2320 Alias for ScriptCallerInfo
* 2286 Add .NET build server shutdown alias "DotNetCoreBuildServerShutdown"
* 2277 Add basic implementation of info command
* 2201 Extend supported globber patterns
* 2200 Support UNC paths
* 2198 Add GlobberSettings
* 2197 Don't rely on System.IO namespace for FilePath/DirectoryPath
* 1976 Add MSBuildSettings.NoLogo
* 1383 Add command line option to display build target graph
* 2342 Provide value for self-contained to support succeeding parameters
* 2310 Cake.Testing.Xunit RuntimeFact and RuntimeTheory doesn't work for .NET Core App
* 2252 Cake fails to start on posix systems if script / current directory is root ( / )
* 2391 Upgrade to NuGet 4.9.2
* 2387 Extend GetTooolExecutableNames for GitVersion
* 2384 Add homebrew fallback path for MSBuild tool resolver
* 2369 Update Roslyn to 2.10.0
* 2350 In-process NuGet client should reuse package sources as specified in NuGet.Config if available
* 2341 Add support for JUnit Output Format
* 2332 TFBuild UploadArtifact commands should support directories
* 2312 Add method to expand environment variables to FilePath/DirectoryPath
* 2308 Use Mono for full framework executables if running on Unix & .NET Core
* 2306 Add VSTestReportPath to DotNetCoreTestSettings
* 2300 Make DotNetCoreTool alias project path optional add overloads without.
* 2297 NUnit3Settings does not provide an option to specify the configuration file to load
* 2284 --version should only return sem/nuget version
* 2272 Update in-process NuGet client to support offline environments
* 2268 Add .NET Core tool support for Octopus aliases
* 2265 Update Roslyn to 2.9.0
* 2257 NuGetPack should have a version suffix setting
* 2255 Show warning when referenced package is missing version number
* 2246 Add NuGet projectUrl to nuspec/csproj packages
* 2245 Add symbols for Cake.Tool package
* 2061 NuGetPack overwrites developmentDependency and requireLicenseAcceptance from nuspec.
* 1875 Folder structure of tools and addins can cause too long paths on Windows
* 2385 Typo in BuildSystem.TeamCity property example
* 2365 Fixed typos
* 2267 Fix more 'occured' and 'occuring' typos

### New in 0.30.0 (Released 2018/08/22)

* 2067 Publish as .NET Core Global Tool.
* 2238 Add repository metadata to NuGet packages.
* 2234 Remove mono argument from Argument Parser.
* 2211 DotNetCorePublishSettings doesn't contain --no-build flag support introduced in .NET Core SDK 2.1.
* 2146 Enabling initializer syntax for all collection properties.
* 1401 Support for dotCover configuration file.
* 2233 Add bootstrap argument to Help Command.
* 2232 Add exclusive argument to Help Command.
* 2220 Incorrect documentation for InnoSetup Alias.
* 2228 CakeTaskExtensions are no longer accessible.
* 2224 Add option for ProcessSettings to opt out of working directory magic.
* 2214 Cake.CoreCLR can't handle whitespace in path.
* 2208 WithCriteria does not work with 'DryRun' (WhatIf flag).
* 2207 NuGet hang due to bug in NuGet 4.6.0.

### New in 0.29.0 (Released 2018/07/06)

* 2140 DotNetCorePublish does not respect SelfContained DotNetCorePublishSettings property.
* 2203 Add Octopus Deploy Promote release support.
* 2095 Add "--skipnontestassemblies" funcionality to Cake's NUnit3Settings as it exists in original NUnit 3 test runner.
* 2094 Add support for executing a single task without dependencies.
* 2196 NuGet Repository information not settable in NuGet Pack.
* 2185 Try to find vswhere.exe on the system if the tool is not registered.
* 2154 Problem with loading abolute path scripts with #load preprocessor.
* 2152 try resolve vstest.console.exe before guessing it.
* 1609 Add additional VSTS actions.
* 2195 Updated the WiX tool documentation.
* 2193 Add Pascal and Dave to all required places.
* 2188 The CLA link in readme seems invalid or broken.

### New in 0.28.1 (Released 2018/06/18)

* 2176 Skipped tasks show up multiple times in report
* 2190 Suppress NuGet dependency warnings related to Cake.Core

### New in 0.28.0 (Released 2018/05/31)

* 2008 Allow defining a typed context to be used throughout a Cake script.
* 1772 Provide access to the run target and ordered list of tasks
* 1594 Add overload to WithCriteria which prints a message
* 2174 Support multiple Support / Teardown
* 2171 Add potential breaking change warning
* 2163 Update to Roslyn 2.8.x packages, adding support for C# 7.3

### New in 0.27.2 (Released 2018/05/15)

* 2137 Dependency loading errors with Cake 0.27.1 and Cake.Powershell 0.4.5
* 2134 Assembly conflicts during compilation

### New in 0.27.1 (Released 2018/04/21)

* 2132 Problems with loading certain assemblies (0.27.0)

### New in 0.27.0 (Released 2018/04/19)

* 2078 Support expand environment variables in script pre-processor directives
* 2047 Specify version during NuGet Updating
* 2005 Add entries for Setup/Teardown in report
* 1908 Octopus Deploy tool does not support list-deployments call for octo.exe
* 2116 Loading Newtonsoft.Json in Cake.CoreCLR throws during assembly loading
* 2084 Cake does not load dependencies in correct order
* 2082 Investigate NuGet local V3 cache
* 2081 Possibility to override default NuGet sources
* 2079 Default sources not loaded if nuget_source is empty
* 2119 DotNetCore Publish misses Force / Self contained / Sources settings
* 2113 Error when loading tools without internet connection
* 2106 Remove NUnit3Settings.ErrorOutputFile property
* 2092 Unable to set 'no-build' and 'no-restore' when executing DotNetCoreRun
* 2051 Add support for msbuild.exe /restore option
* 2039 XUnit2Runner doesn't respect ParallelismOption.None
* 2036 Don't output usage when an error occured.
* 2031 Simplify setting FileVersion and InformationalVersion
* 2029 Investigate in-process NuGet dependency resolution
* 2014 In-process NuGet don’t support multiple feeds through config
* 2003 Add possibility for AssemblyMetadata collection in CreateAssemblyInfo
* 1887 DotNetCoreRestoreSettings: support option --force
* 1557 Add support for MSBuild /consoleloggerparameters
* 2062 Fixed typo 'need to'
* 2035 Fix typo in README
* 1213 NuGetPushSettings.Source: incorrect documentation

### New in 0.26.1 (Released 2018/03/03)

* 2063 Cake running on Mono can't load netstandard 2.0 assembly

### New in 0.26.0 (Released 2018/02/26)

* 1781 Update to .NET Core 2

### New in 0.25.0 (Released 2018/01/17)

* 1995 Make In-proc NuGet addin/tool installation default
* 1994 Get MSBuild Verbosity enum from string
* 1988 TeamCity writing start and end progress contains invalid messages property
* 1974 ToDictionary on Mono causes "The type 'Dictionary<,>' is defined in an assembly that is not referenced"
* 1998 Some .NET Core commands missing no dependencies/restore
* 1997 Add the --trace option to the NUnit3Settings class.
* 1992 Update to .NET Runtime 1.0.9 because security issues
* 1989 Path unnecessarily trims backslash in already normalized string
* 1987 Confusing Error from Bad Format String to Information()
* 1937 UseInProcessClient=true is slow
* 1982 CodeTriage - Get more Open Source Helpers
* 1689 ChocolateyDownload should be documented to only work in paid edition

### New in 0.24.0 (Released 2017/12/29)

* 1950 Allow Cake modules to be bootstrapped by Cake in a pre-processing phase
* 1833 NUnit: Add support for /labels
* 1653 Add Before and After options to NUnit3Labels enum
* 74 MSpec support
* 1957 Use working directory instead if initial script path for resolving tools directory in NuGetLoadDirectiveProvider
* 1939 Bug - TypeExtensions.GetFullName doesn't handle nested types correctly
* 1933 NuGetPackSettings.Properties does not support whitespaces.
* 1930 The "out" parameters are not compiled properly.
* 1915 Only set working directory on process runner if set in settings
* 1889 XmlPoke ignores BOM encoding settings
* 1874 NuGet script load: Do not add include for all cake scripts when include already specified
* 1968 Add interface for AssemblyVerifier so that it can be mocked
* 1960 Update Roslyn to 2.6.1
* 1955 ResultsDirectory is missing from DotNetCoreTestSettings
* 1952 Add support for class/namespace/method arguments for XUnit2
* 1946 Add option to pack files into the NuGet tool directory
* 1943 Chocolatey package dependencies cannot be set using the ChocolateyPackSettings
* 1936 Move to signing service v2
* 1931 Allow passing a nuget.config as environment variable or in cake.config
* 1924 Set UserAgent for in-process NuGet
* 1922 GitVersion is missing AssemblySemFileVer
* 1912 Support for DotCover Process Filter
* 1910 MSBuild property values should escape carriage return and line feed
* 1855 SignTool is not found with latest windows 10 SDK
* 1796 Obsolete DotNetBuild and ultimately remove it
* 1692 Log script compilation warnings and other diagnostics
* 1522 The MSTest tool doesn't pick up the mstest.exe from Visual Studio 2017
* 1811 Add code sample to build system properties

### New in 0.23.0 (Released 2017/10/11)

* 1805 Change GitVersion settings to use nullable integer
* 1856 Support MSBuild warnaserror and warnasmessage arguments
* 1821 Missing Cake method for nuget list
* 1818 Support task dependees (reverse dependencies)
* 1766 Support for #define
* 1032 Support async callbacks
* 1853 The "using static" directive doesn't compile
* 1843 NuGetContentResolver should not return ref assemblies.
* 1842 Params in URI pre-processor directives are case sensitive
* 1838 Dependencies are installed but have no references added when using LoadDependencies=true with in process NuGet client
* 1831 CleanDirectories Throws NullReferenceException When Token Is Null
* 1815 Exception Message should be shown rather than "One or more errors occurred."
* 1404 MsBuildSettings.WithProperty does not escape values
* 1840 Fix Chocolatey Package
* 1804 Unable to execute when namespace-less assembly with CakeMethodAlias is referenced
* 1731 GitLabCI variable changes.
* 1632 Tasks with long names do not display nicely with showdescription
* 1607 ToolResolutionStrategy fails unexpectedly with Cake.LongPath.Module
* 1548 LogExtension colorizes output incorrectly
* 1547 Escaping curly braces in log messages
* 787 Reference NuGet dependencies installed via the #addin directive
* 1835 Fixed typo in installer scripts
* 1814 Fix typo: envrionment

### New in 0.22.2 (Released 2017/09/17)

* 1807 NuGetVersion and CommitDate are null with 0.22.1

### New in 0.22.1 (Released 2017/09/15)

* 1798 GitVersion error on build on master branch with version 0.22.0

### New in 0.22.0 (Released 2017/09/13)

* 1785 Bump LatestBreakingChange to 0.22.0
* 1745 Change parameter for InstallTools and InstallAddins in IScriptProcessor
* 1720 ILRepackSettings.Libs should be List of DirectoryPath
* 1719 Jenkins BRANCH_NAME is missing
* 1714 Updated CakeRuntime.TargetVersion to net462.
* 1674 MSBuildFileLogger LogFile is a string and not a FilePath
* 1665 NUnit3Settings: Params and multiple results
* 1651 NUnit3Settings Verbose flag obsoleted by NUnit console runner
* 1614 Correct the class that TeamCityEnvironmentInfo inherits from
* 1597 CommitsSinceVersionSource and PreReleaseNumber as Integer
* 1564 DeleteDirectory cannot delete read-only files
* 1540 Upgrade to Roslyn 2.0
* 1791 Add option to enable MSBuild binary logging
* 1771 Look for msbuild in default install path on Linux
* 1761 DoesForEach() extension method
* 1754 VSWhere not returning prerelease versions
* 1743 Implement functionality in Cake.NuGet for downloading packages
* 1734 Add GitLink 3 compatible aliases
* 1710 Add alias for simple sub-directory listing
* 1699 NuGetPackSettings missing language/locale ID for the package
* 1670 OpenCover is missing some commandline parameter (for example mergebyhash)
* 1667 Add support for choco download internalize-all-urls
* 1621 Add overload for StartProcess which also returns redircted error output
* 1775 Strange usage of Cake.Core.dll when executing cake sub process
* 1773 NuGetHasSource call do not take care of ArgumentCustomization in NuGetSourcesSettings
* 1759 XmlPoke always writes the xmldeclaration even if the original file didn't have one
* 1742 Some unit tests are locale-sensitive
* 1739 NuGetContentResolver can't find assemblies if located in root
* 1738 NuGetInstaller can't resolve files if package contains dependencies
* 1697 CakeContextAdapter do not implement ICakeContext
* 1694 Addin directive shouldn't attempt to load native assemblies
* 1693 Possible bug when setting process environment variable
* 1625 Comma in msbuild commands are not escaped
* 1602 MSBuildFileLogger Verbosity does not accept Verbosity.Verbose
* 1537 XmlPeek not working correctly for element nodes
* 1422 Error: Unkown token when directory contains multibyte characters
* 1752 Extend DownloadFile to allow AcceptEncoding gzip
* 1746 ScriptAnalyzer.Analyze() should not throw - instead return list of errors
* 1704 Move CakeConsole & CakeBuildLog to Cake.Core and made CakeConfiguration public
* 1512 Please support C*  7 and Roslyn v2
* 753 Tool Versioning
* 1787 Add opt-out config information to assembly version verification error message
* 1780 Fix typo in version.cake
* 1727 Incorrect documentation for XmlPeek Alias
* 1700 Update NuGet license url
* 1525 Updated examples for DotNetCoreTest

### New in 0.21.1 (Released 2017/07/15)

* 1685 Add DotNetCoreTool alias overload that takes DotNetCoreToolSettings parameter
* 1686 AssemblyLoadContext root path is relative

### New in 0.21.0 (Released 2017/07/14)

* 1533 Update DotNetCore Aliases to match tooling 1.0
* 1554 Updated DotNetCoreTest inline with Tooling v1.0
* 1553 Updated DotNetCorePack inline with Tooling v1.0
* 1552 Updated DotNetCorePublish inline with Tooling v1.0
* 1551 Updated DotNetCoreExecute inline with Tooling v1.0
* 1550 Updated DotNetCoreBuild inline with Tooling v1.0
* 1534 Updated DotNetCoreRestore inline with Tooling v1.0
* 1599 Added support for dotnet msbuild inline with Tooling v1.0
* 1591 Add support for choco download
* 1581 Added support for dotnet nuget push inline with Tooling v1.0
* 1577 .NET Core CLI tools - Surfacing additional commands with a new alias: DotNetCoreTool
* 1565 Added support for dotnet nuget delete inline with Tooling v1.0
* 1555 Added support for dotnet clean inline with Tooling v1.0
* 1549 Common changes for DotNetCore Alias
* 1679 Cake on dotnet core doesn't load reference dll correctly if referenced from a subdirectory
* 1673 The xunit.runners package was deprecated
* 1654 Broken Documentation link for ReportUnit

### New in 0.20.0 (Released 2017/06/12)

* 1539 Update solution to Visual Studio 2017
* 1640 Fetch version from solutioninfo & remove newtonsoft dependency
* 1638 Unix Integration tests fail post new SDK
* 1635 Non Nuspec assemblies not packaged after VS2017 upgrade
* 1603 Push Cake.NuGet to MyGet/NuGet
* 1538 Update DotNetInstallerUri to https://dot.net/v1/dotnet-install.ps1
* 1620 Improve documentation for RedirectStandardError and RedirectStandardOutput
* 1613 Added documentation link to NUnit3Settings.Where
* 1605 Fix the contribution-guidelines link again
* 1604 Fix the contribution guidelines link in the README
* 1595 Add Alistair and Martin names to all required places

### New in 0.19.5 (Released 2017/05/04)

* 1587 Arguments missing for Octopus Deploy tool

### New in 0.19.4 (Released 2017/04/19)

* 1556 TeamCity BuildNumber is a string
* 1566 Generic alias methods with type constraints fail compilation

### New in 0.19.3 (Released 2017/04/03)

* 1544 Windows 10 SDK path is not being resolved

### New in 0.19.2 (Released 2017/04/01)

* 1546 MSBuild Logger Path are not correctly quoted

### New in 0.19.1 (Released 2017/03/24)

* 1543 VSWhere aliases should return Directory Paths and not File Paths

### New in 0.19.0 (Released 2017/03/23)

* Add VSWhere support
* Error: SignTool SIGN: Password is required with Certificate path but not specified.
* MSBuild on Mac/Linux
* Categorize logging aliases by level

### New in 0.18.0 (Released 2017/03/07)

* Remove obsoleted DNU aliases
* WiXHeat misleading signature- no mode operates on file list
* Add "build tools" path for MSBuild 2017 to MSBuildResolver
* Add ChocolateyNew Alias
* Add support for NuGet Init and Add commands
* NUnitSettings does not have X86 property
* Enhance TeamCity provider
* Support for TF Build Commands
* Provide ability to add Custom attributes when creating AssemblyInfo
* Support for uninstall packages using Chocolatey
* Provide ability to specify name for xunit report
* MSBuild support for Visual Studio 2017 (aka "15")
* Add support for importing namespaces at the assembly level
* Add DotCover Merge
* Proposal: Allow modules to listen for script lifecycle events
* Support optional parameters on alias methods
* Support downloadable .cake script directive
* Extending the Sign command
* Fix ParseAssemblyInfo does not work .vb
* Duplicate depedencies references in project.json for Cake.Testing.XUnit
* Cake.Testing package depends on xunit.core package
* Optional parameter codegen not invariant
* XBuildRunner#GetToolExecutableNames returning wrong executables
* Space in Reference Preprocessor Directive Throws Illegal characters in path
* Spaces in #load path will cause an Illegal characters in path error.
* Add CakeNamespaceImport for BuildSystem Aliases
* HeatSettings.OutputGroup is unusable
* OctoPack not passing --format to octo.exe
* Error: Unknown Token when directory contains @ character.
* Using reserved name for parameter name causes a parser failure
* signtool.exe should be called only once when signing multiple files
* Missing MSBuild15 on enum NuGetMSBuildVersion for VS 2017
* Add ChocoPush alias for an IEnumerable<FilePath>
* Add ChocoPack alias for an IEnumerable<FilePath>
* Usage of -NoCache on installing tools and addins
* Mac OSX is not properly detected when running on Mono
* NuGet Tool Locator system paths on mac need updating
* Logging throws exception when there are curly braces in the string
* CopyDirectory - Missing Log information
* Teach XmlPeek to silence warnings, if needed
* Http call in unit test
* Add optional Go.CD Server URL Parameter to GetHistory
* Add RedirectStandardError to ProcessRunner
* Cake's default tools / addins / modules paths are not so default as they seem.
* ArgumentException with illegal character information
* Add mechanism to validate addins
* Support XUnit's x86 .exe runner
* Add Gitter and Twitter Notifications
* DownloadFile typo in docs
* Typo in SignTool docs
* Fix typos in GitVersion documentation
* Correct issue with GitLink Alias Category
* Fix commented example for DotNetCoreTest
* Fix doc comments in InnoSetupAliases
* Fix typo in comment
* Fixed Spelling Mistake.

### New in 0.17.0 (Released 2016/11/09)

* Allow custom loggers in the VSTestSettings
* Add support for InnoSetup
* Add a "Prepend" extension for the ProcessArgumentBuilder
* Add Support for the Go.CD build provider
* Add GitLab CI build system support
* Add VSTS build system support
* Wait for AppVeyor process to exit
* Add Ability to Redirect Standard Error on IProcess
* Add option to keep the autogenerated NuSpec file
* IsDependentOn with CakeTaskBuilder parameter
* CopyFiles doesn't respect source directory structure
* Add DotCover Report
* Support OctoPack
* Add support for moving directories
* Typo in VSTestSettings extension method name
* Globber exception when using a path with an exclamation
* Error: An item with the same key has already been added while running Cake from commit hooks
* System time separator is used when Octo DeployAt argument is converted to string
* Unquoted VSTest settings file path
* Globber exception when glob contains %
* GetEntryAssembly can return null, leading to NullReferenceException
* NuGetPack fails if no Files have been specified
* Add support fort Specifying Dependencies for Multi Target package
* Support DefaultCredentials usage for Http Downloads
* Add additional parameters to MSBuild runner
* Add Go.CD build history API call
* Some properties for RoundhouseSettings in Cake.Common.Tools.Roundhouse are not working properly
* Add user agent for DownloadFile
* Guard against invalid path environment variables
* Adding all current parameters for VSTest
* OctoCreateRelease is missing channel option
* Option to deploy an existing release in OctopusDeploy
* Get return code from intercepted process in SpecFlow TestExecutionReport
* Add parameter LogFile to DotCover commands
* Can't specify hash algorithm for the Sign command
* MSBuild add log file support
* Support for SHA256 code signing
* Fixed typos 'occured' and 'occuring'
* Add CLA link to README.
* Removed erroneous apostrophes
* Corrects the grammar "do/does" in exception messages and tests
* Adds default CPU count behavior to MSBuild settings documentation

### New in 0.16.2 (Released 2016/10/11)

* Fixed CakeExecuteScript getting access denied errors on mono/m

### New in 0.16.1 (Released 2016/09/25)

* Issue with debugging in v0.16.0 (.NET Core)
* Add missing assembly properties

### New in 0.16.0 (Released 2016/09/15)

* Change API for registering dependencies with Cake
* Add include & exlude parameters to #tool directive
* Allow passing username and password to DownloadFile alias
* Port to .NET Core
* Publish symbol files
* Add missing MergeOutput Option for OpenCover
* ICakeContainerRegistry missing constraint
* NugetV2Resolver doesn't support new netstandard
* Implement custom logger support for MSBuild
* Support MSBuild logger switches

### New in 0.15.2 (Released 2016/07/29)

* Ensured that WiX candle definitions are enclosed in quotes
* Corrected issue with WixHeat HarvestType Out parameter

### New in 0.15.1 (Released 2016/07/28)

* Corrected Issues found with 0.15.0 AppVeyor updates

### New in 0.15.0 (Released 2016/07/26)

* Add support for adding messages to the AppVeyor build log
* Add environment variable support for Process & Tool
* Add ITeardownContext to Teardown method
* Add OutputPath to ProjectParserResult
* Extend SolutionParserResult with solution folders information
* Add Atlassian Bitbucket Pipelines build system support
* Set ICakeRuntime.TargetFramework to constant
* Do not set /m parameter for MSBuild by default
* AppVeyor.UploadTestResults failing silently
* GetFullName in TypeExtensions.cs not handling arrays correctly
* Allow paths with spaces for OutputDirectory on DotNetCore Aliases
* The GetFiles overload with a predicate doesn't work properly
* Added ability to call SetParameter method on TeamCity Provider
* CakeExecuteScript tool resolution fails if script in parent path
* UploadArtifact support via the AppVeyor provider is incomplete
* Added raw version string property in ReleaseNotes class
* Marked Quiet property on DotNetCoreRestoreSettings as obsolete
* Added -oldstyle support for OpenCover tool
* Added -nofetch support for GitVersion tool
* Add an explicit NuGet source for NuGetPush
* Add parsing of references and project references to the project file parser
* Add configuration for default NuGet source
* Add MD5 checking of packages.config to bootstrappper
* Support results file for MSTest runner
* Support NUnit output format for XUnit2 runner
* NUnit runner: Handle specific non-zero exit codes
* Added examples for Fixie
* Corrected spelling mistake in README.md
* Improved documentation for ProcessSettings Timeout property
* Added documentation for multiple arguments for ToolSettings
* Corrected documentation for GitReleaseManager
* Corrected code example for DotNetCorePackSettings
* Add example documentation to aliases

### New in 0.14.0 (Released 2016/07/11)

* Remove obsoleted XmlPoke Aliases
* ToolSettings should allow should support of exit codes other than 0
* Add support for skipautoprops flag OpenCover Alias
* Support Octopus Deploy Push (octo.exe push)
* Add WiX heat support
* Cake looks for configuration file in the wrong place
* Wrong platform "Any CPU" for project file (expects "AnyCPU")
* Change parameter names passed by GitVersion Alias
* Improve logging with NuGet Install Alias
* Additional null checks for module support
* Suppress obsolete warnings on Mono
* Add known parameters to CakeOptions
* Add working directory to ToolSettings class
* Refactor ICakeEnvironment
* Allow setting `/testsettings:` file for MSTest runner
* Corrected documentation for Directory Alias
* Corrected documentation for DotNetBuild Alias


### New in 0.13.0 (Released 2016/06/07)

* DotNetCoreTest() alias calls DotNetCoreRun()
* Fix DotNet CLI multi-arguments
* Cannot parse AssemblyInfo.cs files generated by MonoDevelop
* Cake.Core.Tooling.ToolRepository.Register(FilePath) path not validated
* Problems using XmlPeek and XmlPoke in XML files with DOCTYPE
* MakeNSIS() does not support filepath for scripts with blanks in path name
* Custom command line switches can't contain spaces in value
* Signtool: Add support for description URL (/du)
* Support uploading test results XML to AppVeyor
* Add support for the vstest.console.exe unit test runner
* Add Module Support
* Add setter for Verbosity in ICakeLog
* Add support for additional arguments to DotNetCoreExecute and DotNetCoreRun
* Allow setting boolean values for built in Cake arguments
* Add DevelopmentDependency to NuGetPackSettings
* Project parser should support TargetFrameworkProfile
* Add tests for Project Parser
* Cake.Core.Tooling namespace isn't documented
* Incorrect documentation in obsolete attribute
* Correct documentation for ILMerge
* Add Summary Documentation for all aliases


### New in 0.12.0 (Released 2016/05/25)

* Fix globalization & white space issue
* New Setup(Action<ICakeContext>)  fails on mono
* Cake.pdb is missing in artifacts
* Tool path is wrong when calling cake file in sub directory
* Add support for .NET Core CLI
* Add pre-processor directive which injects Debugger.Break()
* Add an EnsureDirectoryExists Alias to CreateDirectory and not fail if it exists
* Debugging support
* Added additional missing variables for GitVersion
* Add missing GitVersion return values
* Add missing GitVersion return values
* Consider adding developmentDependency to nuspec for Cake.Common
* Specify culture for Roslyn debug string formats
* Add cake-build build.cake NuGet restore retry handling
* During the publishing cycle, continue with each step, and error at end if there is a problem
* Add all artifacts to AppVeyor during publishing cycle
* Add register and ReturnTargetCodeOffset option in opencover
* Unified tool resolution
* Drop DNU/DNX support in favor of dotnet CLI
* Corrected resource download urls
* Docs: Typos in XBuild docs
* -Mono parameter is not documented in Program.cs output
* How to get ILRepack executable?


### New in 0.11.0 (Released 2016/05/01)

* Regression: ProcessArgumentListExtensions was renamed
* DNU usage of multi arguments changed
* MSTest tool resolution fail if Visual Studio isn't in default location
* Fix issue with final build step
* Actual type of RepositoryUrl is String not Uri
* Add configuration file for Cake.
* Add Text Transform support
* Add FileSize alias
* Add TravisCI buildsystem
* Add integration tests
* Remove tools and addins from packages.config.
* Setup Issue and Pull Request Templates
* Obsolete XmlPoke string alias and add new method/alias for string
* Signtool: Add support for certificates from the certificate store based on thumbprint
* Add generic optional tool timeout
* Add NuGetPacker support for IncludeReferencedProjects
* Parse multiple InternalsVisibleTo attributes from AssemblyInfo
* Add StorePasswordInClearText to NuGetSoiurceAdd NuGetSourcesSettings
* TeamCity ImportDotCoverCoverage tests fail when running on TeamCity
* Have TeamCityDisposableExtensions extend ITeamCityProvider
* Remove (Install) from Chocolatey Package
* Setup/Teardown should provide ICakeContext
* Signtool: Add support for description (/d)
* Support NuGet 3 new parameters
* Added note about ReSharper License
* Added Chocolatey Package Badge


### New in 0.10.1 (Released 2016/04/07)

* Exception running InspectCode and then directly after TeamCity.ImportData
* Ensure Cake Assemblies are stamped with current version number
* Clean up build script for Cake

### New in 0.10.0 (Released 2016/03/16)

* XUnit command line bug
* Cake does not find its own nuget.exe on Linux
* Sanitization in TeamCity Provider places extra apostrophe if '[' is used.
* Path segment bug (or test bug, choose your own adventure!)
* Add support for importing coverage to TeamCity
* Add DotCover Cover support
* Add SpecFlow support
* Add Jenkins CI build system support
* Use V3 NuGet in bootstrapper
* Remove logging from task setup/teardown.
* Update ReleaseNotes.md
* Removed year from © in readme
* Add GitVersion into build.cake
* TextTransformation.Save creates BOM on new file

### New in 0.9.0 (Released 2016/02/22)

* Add missing command line switch for ILRepack's targetplatform
* DNU tools only work on windows
* Change Nuget.Core.dll to NuGet.Core.dll in build script
* ScriptAliasFinder fails on Mono when assembly isn't loaded in FindAliases
* FileExists alias in should make the provided path absolute
* Add support for dotCover
* Add Continua CI build system support
* Build release branches on AppVeyor
* Add Unit Tests for ILRepackRunner
* OpenCover tool only intercepts the last test assembly
* Update license to remove year, as this is not required
* Support for Properties argument in nuget pack
* Extend NuGet aliases
* Corrected Class Name Associated with Unit Test
* Add DotNetBuild settings extension methods and tests
* Replace #if !UNIX with [WindowsFact]
* Don't show delegating tasks in summary
* Task Summary should include skipped tasks
* Support for NuGet packing of project files
* Add method to get relative paths (for paths)
* Full Build/Publish Automation for Cake
* Add GitVersion into build.cake
* Tool: Write exit code to log
* Allow use of ICakeContext in WithCriteria
* Command line parameters should follow conventions

### New in 0.8.0 (Released 2015/01/18)

* DNUPackSettings OutputDirectory should be a DirectoryPath
* Add GitLink Alias
* Make #tool and #addin package manager agnostic
* XmlPeek alias
* Move from WebClient to HttpClient
* PlatformTarget is missing Win32
* Move ToolFixture to Cake.Testing
* Line number in error messages is incorrect when using directives

### New in 0.7.0 (Released 2015/12/23)

* CakeBuildLog ConsolePalette missing LogLevel.Fatal map
* StartProcess hangs sometimes with large input
* Log errors to console standard error
* Support arbitrary text when parsing AssemblyInformationalVersion.
* Run unit tests on Travis
* Use OutputDirectory property in Choco Pack for Cake
* Workarounds for incomplete tool settings
* Adding support for Atlasssian Bamboo Build Server
* Added missing CakeAliasCategory attribute
* Add code of conduct

### New in 0.6.4 (Released 2015/12/09)
* Quoted process fails on unix

### New in 0.6.3 (Released 2015/12/07)
* ProcessStartInfo filename not always Quoted
* Support spaces in MSBuild configuration
* Add support for DNU

### New in 0.6.2 (Released 2015/12/03)
* Added fix for getting current framework name on Mono.

### New in 0.6.1 (Released 2015/12/02)
* Addded NUnit 3 support.
* Added MSBuild support for ARM processor.
* Added support to deprecate aliases.
* Added new AppVeyor environment variable (job name).
* Added support for MSBuild platform architecture.
* Added output directory for ChocolateyPack.
* Corrected parameter passed to Create method of GitReleaseManager.
* Fixed misconfiguration in GitVersion Runner.
* Fixed null reference exception being thrown when analyzing ReSharper CLI reports.
* ComVisible Attribute was not being parsed correctly by AssemblyInfoParseResult.
* Fixed globber exception when path has ampersand.
* CopyFile logged incorrect target file path.
* ParseAssemblyInfo ignored commented information.
* Got support for .cake files in GitHub.
* Created a Visual Studio Code extension for Cake.
* Created a VSTS extension for Cake.
* Fixed issue with external NuGet packages used directly via #addin directive.
* DupFinder: Added ability to fail the build on detected issues.
* InspectCode: Added ability to fail the build on detected issues.
* TextTransform now handles Regex special characters.

### New in 0.6.0 (Released 2015/11/04)
* Added Chocolatey support.
* Added GitReleaseManager support.
* Added GitReleaseNotes support.
* Added GitVersion support.
* Added MyGet build system support.
* Added OpenCover support.
* Added ReportGenerator support.
* Added ReportUnit support.
* Added Cake script analyzer support.
* Extended AssemblyInfo parser.
* Extended ProcessArgumentBuilder with switch.
* Extended TeamCity build system support.
* Improved NuGet release notes handling.
* Refactored Cake Tool handling & tests.

### New in 0.5.5 (Released 2015/10/12)
* Added alias to retrieve all environment variables.
* Added additional xUnit settings.
* Added verbose message when glob pattern did not match anything.
* Added task setup/teardown functionality.
* Fix for referencing parent directory in glob patterns.
* Added verbose logging for file and directory aliases.
* Removed quotes from MSBuild arguments.
* Added StartProcess alias overload taking process arguments as string.
* Added Cake.Testing NuGet package.
* Added support for AssemblyConfiguration when patching assembly information.
* Fixed bug with dots in glob patterns.
* Fixed bug with reference loading (affects #tool and #addin directives).

### New in 0.5.4 (Released 2015/09/12)
* Removed .nuspec requirement for NuGetPack.
* Enhanced exception message to include name of missing argument.
* Extended ProcessAliases with methods returning IProcess.
* Added string formatting for process argument builder.
* Added path to NuGet resolver for Mono on OS X 10.11.
* Added Homebrew install paths to Cake tool resolver.
* Changed NUnit argument prefix from '/' to '-'.
* Restored accidental sematic change with globber predicates.

### New in 0.5.3 (Released 2015/08/31)
* Additional NUnit switches.
* Made IProcess disposable and added Kill method.
* Fix for glob paths containing parentheses.
* Fix for MSBuild Platform target.
* xUnit: Added support for -noappdomain option.
* DupFinder support added.
* InspectCode Support added.

### New in 0.5.2 (Released 2015/08/11)
* Globber performance improvements.
* Increased visibility of skipped tasks.
* Added ILRepack support.
* Fix for PlatformTarget not used in MSBuild runner.
* Changed TeamCityOutput to a nullable boolean.
* Fix for CleanDirectory bug.
* Added support for using-alias-directives (Roslyn only).
* Added XmlPoke support.

### New in 0.5.1 (Released 2015/07/27)
* Increased stability when running on Mono.
* Added MSTest support for Visual Studio 2015 (version 14.0).
* Renamed MSOrXBuild to DotNetBuild.
* Better error reporting on Mono.
* Fixed path bug affecting non Windows systems.
* Cake now logs a warning if an assembly can't be loaded.

### New in 0.5.0 (Released 2015/07/20)
* Added Mono support.
* Added XBuild alias.
* Improved tool resolution.
* Added Fixie support.
* Added IsRunningOnWindows() alias.
* Added IsRunningOnUnix() alias.
* Added NuGet proxy support.
* Fixed MSBuild verbosity bug.
* Added shebang line support.

### New in 0.4.3 (Released 2015/07/05)
* Added TeamCity support.
* Added filter predicate to globber and clean directory methods.
* Added Unzip alias.
* Added DownloadFile alias.
* Added method to retrieve filename without its extension.
* Added support for InternalsVisibleToAttribute when generating assembly info.
* Added extension methods to ProcessSettings.
* Fixed formatting in build report.
* Fixed problems with whitespace in arguments.

### New in 0.4.2 (Released 2015/05/27)
* Added aliases for making paths absolute.
* Added support for creating Octopus Deploy releases.

### New in 0.4.1 (Released 2015/05/18)
* Made Cake work on .NET 4.6 again without experimental flag.
* The tools directory now have higher precedence than environment paths when resolving nuget.exe.

### New in 0.4.0 (Released 2015/05/12)
* Now using RC2 of Roslyn from NuGet since MyGet distribution was no longer compatible.
* Added support for MSBuild 14.0.

### New in 0.3.2 (Released 2015/04/16)
* NuGet package issue fix.

### New in 0.3.1 (Released 2015/04/16)
* Fixed an issue where Roslyn assemblies weren't loaded properly after install.

### New in 0.3.0 (Released 2015/04/16)
* Added experimental support for nightly build of Roslyn.
* Fixed an issue where passing multiple assemblies to NUnit resulted in multiple executions of NUnit.
* Added Windows 10 OS support.

### New in 0.2.2 (Released 2015/03/31)
* Added lots of example code.
* Added target platform option to ILMerge tool.
* Added #tool line directive.
* Added support for NuGet update command.

### New in 0.2.1 (Released 2015/03/17)
* Added convertable paths and removed path add operators.

### New in 0.2.0 (Released 2015/03/15)
* Added script dry run option.
* Added MSBuild verbosity setting.
* Added convenience aliases for working with directory and file paths.
* Fixed console rendering bug.
* Fixed nuspec xpath bug.
* Fixed parsing of command line arguments.

### New in 0.1.34 (Released 2015/03/03)
* Added support for NuGet SetApiKey.
* Fixed unsafe logging.
* Made text transformation placeholders configurable.
* Added missing common special paths.
* Fixed script path bug.
* Added XML transformation support.

### New in 0.1.33 (Released 2015/02/24)
* Added Multiple Assembly Support.
* Added process output and timeout.
* Fixed code generation issue.
* Added aliases for executing cake scripts out of process.
* Added file hash calculator.
* Added aliases for checking existence of directories and files.
* Added support for NSIS.

### New in 0.1.32 (Released 2015/02/10)
* Fixed issue where script hosts had been made internal by mistake.

### New in 0.1.31 (Released 2015/02/10)
* Documentation updates only.

### New in 0.1.30 (Released 2015/02/08)
* Added support for installing NuGet packages from script.
* Added filter support to CleanDirectory.

### New in 0.1.29 (Released 2015/01/28)
* Fixed globber bug that prevented NUnit runner from running.

### New in 0.1.28 (Released 2015/01/18)
* Added support for transforming nuspec files.
* Added support for copying directories.

### New in 0.1.27 (Released 2015/01/13)
* Made build log easier to read.
* Fixed wrong namespace for CLSCompliant attribute.
* Added predictable encoding to AssemblyInfoCreator.

### New in 0.1.26 (Released 2015/01/11)
* Added AppVeyor support.
* Added #addin directive for NuGet addins.
* Added assembly company to AssemblyInfoCreator.
* Added finally handler for tasks.
* Added error reporter for tasks.

### New in 0.1.25 (Released 2015/01/01)
* Added parsing of solution version information if available.
* Fixed so logging won't throw an exception if one of the arguments is null.
* Fix for argument parsing without script.
* Added support for simple text transformations.

### New in 0.1.24 (Released 2014/12/12)
* Added support for NuGet sources.
* Added solution and project parsers.

### New in 0.1.23 (Released 2014/11/21)
* Removed silent flag from xUnit.net v2 runner since it's been deprecated.

### New in 0.1.22 (Released 2014/11/20)
* Added support for script setup/teardown.
* Added MSBuild node reuse option.
* Added xUnit.net v2 support.

### New in 0.1.21 (Released 2014/09/23)
* Added line directives to generated scripts.

### New in 0.1.20 (Released 2014/09/14)
* Fix for relative paths in Globber.
* Specifying a script now take precedence over version or help commands.
* Throws if target cannot be reached due to constraints.
* Added logging when tasks are skipped due to constraints.
* Changed location of transformed nuspec file.
* Made nuspec XML namespaces optional.

### New in 0.1.19 (Released 2014/09/03)
* Added default file convention recognizer.
* Added assembly info parser.
* Added error handling.
* Added total duration to task report.
* Added Sign extension for assembly certificate signing.
* Changed the way processes are started.
* Now outputs full stack trace in diagnostic mode.
* Fixed issue with relative paths in tools.
* Added xUnit silent flag.

### New in 0.1.18 (Released 2014/08/21)
* Added external script loading.
* IFile.OpenWrite will now truncate existing file.
* Added overloads for common script alias methods.
* Added support for running custom processes.
* MSBuild runner now uses latest MSBuild version if not explicitly specified.
* Moved Tool<T> to Cake.Core.
* Ignored errors are now logged.
* Added more NUnit settings.
* Added environment variable script aliases.

### New in 0.1.17 (Released 2014/07/29)
* Made non interactive mode mandatory for NuGet restore.
* Added missing Cake.Common.xml.
* Major refactoring of tools.
* Added attributes for documentation.

### New in 0.1.16 (Released 2014/07/23)
* Added WiX support.
* Added .nuspec metadata manipulation support to NuGet package creation.

### New in 0.1.15 (Released 2014/07/20)
* Added NuGet push support.

### New in 0.1.14 (Released 2014/07/17)
* Added Cake.Core NuGet package.
* Added support for loading external script aliases.

### New in 0.1.13 (Released 2014/07/10)
* No more logging when creating script aliases.

### New in 0.1.12 (Released 2014/07/10)
* Added file deletion.
* Added file moving.
* Added directory creation.
* Added version command.
* Major refactoring of Cake (console application).
* NuGet packer now use absolute paths.
* Minor fix for console background colors.
* Added way of retrieving environment variables.
* Added script alias property support.

### New in 0.1.11 (Released 2014/07/01)
* Critical bug fix for script host.

### New in 0.1.10 (Released 2014/07/01)
* Added parsing of FAKE's release notes format.
* Added task description support.
* Added script methods for log.

### New in 0.1.9 (Releases 2014/06/28)
* Added AssemblyInfo creator.
* Zip: Fixed bug with relative paths.
* MSBuild: Added support for max CPU count.
* Added logging of process launch parameters.
* MSBuild: Fix for multiple property values & quotation.
* Fixed issue with cleaning deep dir structures.

### New in 0.1.8 (Released 2014/06/25)
* Added NuGet restore support.
* Task names are no longer case sensitive.
* Bug fix for non quoted MSBuild solution argument.
* Added custom collections for file and directory paths.

### New in 0.1.7 (Released 2014/06/21)
* Renamed method Run to RunTarget.
* Various fixes and improvements.

### New in 0.1.6 (Released 2014/06/18)
* Added MSTest support.

### New in 0.1.5 (Released 2014/06/17)
* Added ILMerge support.

### New in 0.1.4 (Released 2014/06/15)
* Added NUnit support.

### New in 0.1.3 (Released 2014/06/14)
* Fixed compression bug where sub directories were not properly included in zip file.

### New in 0.1.2 (Released 2014/06/13)
* Fixed bug where globbing did not take OS case sensitivity into account.

### New in 0.1.1 (Released 2014/06/12)
* Added NuGet Symbol support.
* Restructured solution. Removed individual assemblies and introduced Cake.Common.dll.

### New in 0.1.0 (Released 2014/06/11)
* Added extensions methods for opening files.
* Added task report.
* Minor fix for cleaning directories.

### New in 0.0.8 (Released 2014/06/10)
* Added xUnit options.
* Copying files now overwrite the destination files.

### New in 0.0.7 (Released 2014/06/10)
* Added zip compression support.
* Added NuGet packing support.
* Added file copy convenience methods.

### New in 0.0.6 (Released 2014/06/06)
* Added basic IO functionality such as cleaning and deleting directories.
* Added script host methods for built in functionality (MSBuild, xUnit and Globbing).

### New in 0.0.5 (Released 2014/06/06)
* Added support for MSBuild tool version.
* Added support for MSBuild platform target.

### New in 0.0.4 (Released 2014/06/05)
* Added script argument support.

### New in 0.0.3 (Released 2014/06/04)
* Bug fix for when resolving working directory.

### New in 0.0.2 (Released 2014/06/04)
* Added logging support.
* Added dedicated script runner.

### New in 0.0.1 (Released 2014/05/06)
* First release of Cake.
