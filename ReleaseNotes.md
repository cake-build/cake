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