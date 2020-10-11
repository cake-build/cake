// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Implementation of a script processor.
    /// </summary>
    public sealed class ScriptProcessor : IScriptProcessor
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly IToolLocator _tools;
        private readonly List<IPackageInstaller> _installers;
        private readonly bool _skipPackageVersionCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessor"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="installers">The available package installers.</param>
        /// <param name="configuration">The configuration.</param>
        public ScriptProcessor(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log,
            IToolLocator tools,
            IEnumerable<IPackageInstaller> installers,
            ICakeConfiguration configuration)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            if (installers == null)
            {
                throw new ArgumentNullException(nameof(installers));
            }

            _environment = environment;
            _log = log;
            _tools = tools;
            _installers = new List<IPackageInstaller>(installers);
            var skip = configuration.GetValue(Constants.Settings.SkipPackageVersionCheck);
            _skipPackageVersionCheck = skip != null && skip.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Installs the addins.
        /// </summary>
        /// <param name="addins">The addins to install.</param>
        /// <param name="installPath">The install path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        public IReadOnlyList<FilePath> InstallAddins(
            IReadOnlyCollection<PackageReference> addins,
            DirectoryPath installPath)
        {
            if (addins == null)
            {
                throw new ArgumentNullException(nameof(addins));
            }
            if (installPath == null)
            {
                throw new ArgumentNullException(nameof(installPath));
            }

            // Make the installation root absolute.
            installPath = installPath.MakeAbsolute(_environment);

            var result = new HashSet<FilePath>(PathComparer.Default);
            if (addins.Count > 0)
            {
                _log.Verbose("Installing addins...");
                foreach (var addin in addins)
                {
                    CheckPackageVersion(addin, "addin");

                    // Get the installer.
                    var installer = GetInstaller(addin, PackageType.Addin);
                    if (installer == null)
                    {
                        const string format = "Could not find an installer for the '{0}' scheme.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, addin.Scheme);
                        throw new CakeException(message);
                    }

                    var assemblies = installer.Install(addin, PackageType.Addin, installPath);
                    if (assemblies.Count == 0)
                    {
                        const string format = "Failed to install addin '{0}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, addin.Package);
                        throw new CakeException(message);
                    }

                    // Reference found assemblies.
                    foreach (var assembly in assemblies)
                    {
                        _log.Debug("The addin {0} will reference {1}.", addin.Package, assembly.Path.GetFilename());
                        result.Add(assembly.Path);
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Installs the tools.
        /// </summary>
        /// <param name="tools">The tools to install.</param>
        /// <param name="installPath">The install path.</param>
        public void InstallTools(
            IReadOnlyCollection<PackageReference> tools,
            DirectoryPath installPath)
        {
            if (tools == null)
            {
                throw new ArgumentNullException(nameof(tools));
            }
            if (installPath == null)
            {
                throw new ArgumentNullException(nameof(installPath));
            }
            InstallPackages(tools, installPath, PackageType.Tool);
        }

        /// <summary>
        /// Installs the modules.
        /// </summary>
        /// <param name="modules">The modules to install.</param>
        /// <param name="installPath">The install path.</param>
        public void InstallModules(
            IReadOnlyCollection<PackageReference> modules,
            DirectoryPath installPath)
        {
            if (modules == null)
            {
                throw new ArgumentNullException(nameof(modules));
            }
            if (installPath == null)
            {
                throw new ArgumentNullException(nameof(installPath));
            }
            InstallPackages(modules, installPath, PackageType.Module);
        }

        private IPackageInstaller GetInstaller(PackageReference package, PackageType type)
        {
            foreach (var installer in _installers)
            {
                if (installer.CanInstall(package, type))
                {
                    return installer;
                }
            }
            return null;
        }

        private void InstallPackages(
            IReadOnlyCollection<PackageReference> modules,
            DirectoryPath installPath,
            PackageType packageType)
        {
            if (packageType != PackageType.Tool && packageType != PackageType.Module)
            {
                throw new ArgumentException("Package is not a tool or a module.", nameof(packageType));
            }

            // Make the installation root absolute.
            installPath = installPath.MakeAbsolute(_environment);

            if (modules.Count > 0)
            {
                var packageTypeName = packageType == PackageType.Tool ? "tool" : "module";

                _log.Verbose($"Installing {packageTypeName}s...");
                foreach (var tool in modules)
                {
                    CheckPackageVersion(tool, packageTypeName);

                    // Get the installer.
                    var installer = GetInstaller(tool, packageType);
                    if (installer == null)
                    {
                        const string format = "Could not find an installer for the '{0}' scheme.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, tool.Scheme);
                        throw new CakeException(message);
                    }

                    // Install the module.
                    var result = installer.Install(tool, packageType, installPath);
                    if (result.Count == 0)
                    {
                        var format = $"Failed to install {packageTypeName} '{{0}}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, tool.Package);
                        throw new CakeException(message);
                    }

                    if (packageType == PackageType.Tool)
                    {
                        // Register the tools.
                        foreach (var item in result)
                        {
                            _tools.RegisterFile(item.Path);
                        }
                    }
                }
            }
        }

        private void CheckPackageVersion(PackageReference packageReference, string directiveName)
        {
            bool existsVersionParameter = packageReference.Parameters.Any(x => x.Key.Equals("version", StringComparison.OrdinalIgnoreCase));
            if (!existsVersionParameter && !_skipPackageVersionCheck)
            {
                const string message = "The '{0}' directive is attempting to install the '{1}' package \r\n" +
                                       "without specifying a package version number.  \r\n" +
                                       "More information on this can be found at https://cakebuild.net/docs/writing-builds/reproducible-builds/ \r\n" +
                                       "It's not recommended, but you can explicitly override this warning \r\n" +
                                       "by configuring the Skip Package Version Check setting to true \r\n" +
                                       "(i.e. command line parameter \"--settings_skippackageversioncheck=true\", \r\n" +
                                       "environment variable \"CAKE_SETTINGS_SKIPPACKAGEVERSIONCHECK=true\", \r\n" +
                                       "read more about configuration at https://cakebuild.net/docs/running-builds/configuration/)";

                _log.Warning(Verbosity.Minimal, message, directiveName, packageReference.Package);
            }
        }
    }
}