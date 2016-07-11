// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessor"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="installers">The available package installers.</param>
        public ScriptProcessor(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log,
            IToolLocator tools,
            IEnumerable<IPackageInstaller> installers)
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
            if (installers == null)
            {
                throw new ArgumentNullException("installers");
            }

            _environment = environment;
            _log = log;
            _tools = tools;
            _installers = new List<IPackageInstaller>(installers);
        }

        /// <summary>
        /// Installs the addins.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installPath">The install path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        public IReadOnlyList<FilePath> InstallAddins(
            ScriptAnalyzerResult analyzerResult,
            DirectoryPath installPath)
        {
            if (analyzerResult == null)
            {
                throw new ArgumentNullException("analyzerResult");
            }
            if (installPath == null)
            {
                throw new ArgumentNullException("installPath");
            }

            // Make the installation root absolute.
            installPath = installPath.MakeAbsolute(_environment);

            var result = new HashSet<FilePath>(PathComparer.Default);
            if (analyzerResult.Addins.Count > 0)
            {
                _log.Verbose("Installing addins...");
                foreach (var addin in analyzerResult.Addins)
                {
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
        /// Installs the tools specified in the build scripts.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installPath">The install path.</param>
        public void InstallTools(
            ScriptAnalyzerResult analyzerResult,
            DirectoryPath installPath)
        {
            if (analyzerResult == null)
            {
                throw new ArgumentNullException("analyzerResult");
            }
            if (installPath == null)
            {
                throw new ArgumentNullException("installPath");
            }

            // Make the installation root absolute.
            installPath = installPath.MakeAbsolute(_environment);

            if (analyzerResult.Tools.Count > 0)
            {
                _log.Verbose("Installing tools...");
                foreach (var tool in analyzerResult.Tools)
                {
                    // Get the installer.
                    var installer = GetInstaller(tool, PackageType.Tool);
                    if (installer == null)
                    {
                        const string format = "Could not find an installer for the '{0}' scheme.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, tool.Scheme);
                        throw new CakeException(message);
                    }

                    // Install the tool.
                    var result = installer.Install(tool, PackageType.Tool, installPath);
                    if (result.Count == 0)
                    {
                        const string format = "Failed to install tool '{0}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, tool.Package);
                        throw new CakeException(message);
                    }

                    // Register the tools.
                    foreach (var item in result)
                    {
                        _tools.RegisterFile(item.Path);
                    }
                }
            }
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
    }
}
