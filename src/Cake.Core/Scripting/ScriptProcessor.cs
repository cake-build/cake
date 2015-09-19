using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Implementation of a script processor.
    /// </summary>
    public sealed class ScriptProcessor : IScriptProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly INuGetPackageInstaller _installer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessor"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="installer">The NuGet package installer.</param>
        public ScriptProcessor(
            IFileSystem fileSystem, 
            ICakeEnvironment environment,
            ICakeLog log, 
            INuGetPackageInstaller installer)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (installer == null)
            {
                throw new ArgumentNullException("installer");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _installer = installer;
        }

        /// <summary>
        /// Installs the addins.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installationRoot">The installation root path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        public IReadOnlyList<FilePath> InstallAddins(
            ScriptAnalyzerResult analyzerResult,
            DirectoryPath installationRoot)
        {
            if (analyzerResult == null)
            {
                throw new ArgumentNullException("analyzerResult");
            }
            if (installationRoot == null)
            {
                throw new ArgumentNullException("installationRoot");
            }

            // Make the installation root absolute.
            installationRoot = installationRoot.MakeAbsolute(_environment);

            var result = new List<FilePath>();
            if (analyzerResult.Addins.Count > 0)
            {
                _log.Verbose("Installing addins...");
                foreach (var addin in analyzerResult.Addins)
                {
                    var addInAssemblies = InstallPackage(addin, installationRoot, GetAddInAssemblies);
                    if (addInAssemblies.Length == 0)
                    {
                        const string format = "Failed to install addin '{0}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, addin.PackageId);
                        throw new CakeException(message);
                    }

                    // Reference found assemblies.
                    foreach (var assemblyPath in addInAssemblies)
                    {
                        _log.Debug("The addin {0} will reference {1}.", addin.PackageId, assemblyPath.Path.GetFilename());
                        result.Add(assemblyPath.Path);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Installs the tools specified in the build scripts.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installationRoot">The installation root path.</param>
        public void InstallTools(
            ScriptAnalyzerResult analyzerResult, 
            DirectoryPath installationRoot)
        {
            if (analyzerResult == null)
            {
                throw new ArgumentNullException("analyzerResult");
            }
            if (installationRoot == null)
            {
                throw new ArgumentNullException("installationRoot");
            }

            // Make the installation root absolute.
            installationRoot = installationRoot.MakeAbsolute(_environment);

            if (analyzerResult.Tools.Count > 0)
            {
                _log.Verbose("Installing tools...");
                foreach (var addin in analyzerResult.Tools)
                {
                    var toolExecutables = InstallPackage(addin, installationRoot, GetToolExecutables);
                    if (toolExecutables.Length == 0)
                    {
                        const string format = "Failed to install tool '{0}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, addin.PackageId);
                        throw new CakeException(message);
                    }
                }
            }
        }

        private IFile[] InstallPackage(
            NuGetPackage package, 
            DirectoryPath installationRoot, 
            Func<DirectoryPath, IFile[]> fetcher)
        {
            var root = _fileSystem.GetDirectory(installationRoot);
            var packagePath = installationRoot.Combine(package.PackageId);

            // Create the addin directory if it doesn't exist.
            if (!root.Exists)
            {
                _log.Debug("Creating addin directory {0}", installationRoot);
                root.Create();
            }

            // Fetch available content from disc.
            var content = fetcher(packagePath);
            if (content.Any())
            {
                _log.Debug("Package {0} has already been installed.", package.PackageId);
                return content;
            }

            // Install the package.
            _log.Debug("Installing package {0}...", package.PackageId);
            _installer.InstallPackage(package, installationRoot);

            // Return the files.
            return fetcher(packagePath);
        }

        private IFile[] GetAddInAssemblies(DirectoryPath addInDirectoryPath)
        {
            var addInDirectory = _fileSystem.GetDirectory(addInDirectoryPath);
            return addInDirectory.Exists
                ? addInDirectory.GetFiles("*.dll", SearchScope.Recursive)
                    .Where(file => !file.Path.FullPath.EndsWith("Cake.Core.dll", StringComparison.OrdinalIgnoreCase))
                    .ToArray()
                : new IFile[0];
        }

        private IFile[] GetToolExecutables(DirectoryPath toolDirectoryPath)
        {
            var toolDirectory = _fileSystem.GetDirectory(toolDirectoryPath);
            return toolDirectory.Exists
                ? toolDirectory.GetFiles("*.exe", SearchScope.Recursive)
                    .ToArray()
                : new IFile[0];
        }
    }
}
