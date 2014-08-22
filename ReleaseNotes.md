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