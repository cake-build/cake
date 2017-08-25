### New on 0.21.1 (Released 2017/07/15)

* 1685 Add DotNetCoreTool alias overload that takes DotNetCoreToolSettings parameter
* 1686 AssemblyLoadContext root path is relative

### New on 0.21.0 (Released 2017/07/14)

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

### New on 0.20.0 (Released 2017/06/12)

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

### New on 0.19.5 (Released 2017/05/04)

* 1587 Arguments missing for Octopus Deploy tool

### New on 0.19.4 (Released 2017/04/19)

* 1556 TeamCity BuildNumber is a string
* 1566 Generic alias methods with type constraints fail compilation

### New on 0.19.3 (Released 2017/04/03)

* 1544 Windows 10 SDK path is not being resolved

### New on 0.19.2 (Released 2017/04/01)

* 1546 MSBuild Logger Path are not correctly quoted

### New on 0.19.1 (Released 2017/03/24)

* 1543 VSWhere aliases should return Directory Paths and not File Paths

### New on 0.19.0 (Released 2017/03/23)

* Add VSWhere support
* Error: SignTool SIGN: Password is required with Certificate path but not specified.
* MSBuild on Mac/Linux
* Categorize logging aliases by level

### New on 0.18.0 (Released 2017/03/07)

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

### New on 0.17.0 (Released 2016/11/09)

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

### New on 0.16.2 (Released 2016/10/11)

* Fixed CakeExecuteScript getting access denied errors on mono/m

### New on 0.16.1 (Released 2016/09/25)

* Issue with debugging in v0.16.0 (.NET Core)
* Add missing assembly properties

### New on 0.16.0 (Released 2016/09/15)

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

### New on 0.15.2 (Released 2016/07/29)

* Ensured that WiX candle definitions are enclosed in quotes
* Corrected issue with WixHeat HarvestType Out parameter

### New on 0.15.1 (Released 2016/07/28)

* Corrected Issues found with 0.15.0 AppVeyor updates

### New on 0.15.0 (Released 2016/07/26)

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

### New on 0.14.0 (Released 2016/07/11)

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


### New on 0.13.0 (Released 2016/06/07)

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


### New on 0.12.0 (Released 2016/05/25)

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


### New on 0.11.0 (Released 2016/05/01)

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


### New on 0.10.1 (Released 2016/04/07)

* Exception running InspectCode and then directly after TeamCity.ImportData
* Ensure Cake Assemblies are stamped with current version number
* Clean up build script for Cake

### New in 0.10.0 (Released 2016/03/16)

* XUnit command line bug
* Cake does not find it's own nuget.exe on Linux
* Sanitization in TeamCity Provider places extra apostrophe if '[' is used.
* Path segment bug (or test bug, choose your own adventure!)
* Add support for importing coverage to TeamCity
* Add DotCover Cover support
* Add SpecFlow support
* Add Jenkins CI build system support
* Use V3 Nuget in bootstrapper
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
* Support for nuget packing of project files
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
* Fixed issue with external nugets used directly via #addin directive.
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
* Added method to retrieve filename without it's extension.
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
