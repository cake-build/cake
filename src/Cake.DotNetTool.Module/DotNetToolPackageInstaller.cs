﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// Installer for dotnet Tool Packages.
    /// </summary>
    public sealed class DotNetToolPackageInstaller : IPackageInstaller
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _log;
        private readonly IDotNetToolContentResolver _contentResolver;
        private readonly ICakeConfiguration _config;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetToolPackageInstaller"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        /// <param name="contentResolver">The DotNetTool Package Content Resolver.</param>
        /// <param name="config">the configuration.</param>
        /// <param name="fileSystem">The file system.</param>
        public DotNetToolPackageInstaller(ICakeEnvironment environment, IProcessRunner processRunner, ICakeLog log, IDotNetToolContentResolver contentResolver, ICakeConfiguration config, IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(environment);

            ArgumentNullException.ThrowIfNull(processRunner);

            ArgumentNullException.ThrowIfNull(log);

            ArgumentNullException.ThrowIfNull(contentResolver);

            _environment = environment;
            _processRunner = processRunner;
            _log = log;
            _contentResolver = contentResolver;
            _config = config;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <returns><c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.</returns>
        public bool CanInstall(PackageReference package, PackageType type)
        {
            ArgumentNullException.ThrowIfNull(package);

            return package.Scheme.Equals("dotnet", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <param name="package">The package reference.</param>
        /// <param name="type">The package type.</param>
        /// <param name="path">The location where to install the package.</param>
        /// <returns>The installed files.</returns>
        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            ArgumentNullException.ThrowIfNull(package);

            ArgumentNullException.ThrowIfNull(path);

            // We are going to assume that the default install location is the
            // currently configured location for the Cake Tools Folder
            var toolsFolderDirectoryPath = _config.GetToolPath(_environment.WorkingDirectory, _environment);
            _log.Debug("Configured Tools Folder: {0}", toolsFolderDirectoryPath);

            var toolLocation = toolsFolderDirectoryPath.FullPath;
            if (package.Parameters.ContainsKey("global"))
            {
                toolLocation = "global";
            }
            else if (package.Parameters.ContainsKey("local"))
            {
                toolLocation = "local";
            }

            // First we need to check if the Tool is already installed
            var installedTools = GetInstalledTools(toolLocation);

            _log.Debug("Checking for tool: {0}", package.Package.ToLowerInvariant());

            var installedTool = installedTools.FirstOrDefault(t => string.Equals(t.Id, package.Package, StringComparison.InvariantCultureIgnoreCase));

            if (installedTool != null)
            {
                // The tool is already installed, so need to check if requested version is the same as
                // what is already installed
                string requestedVersion = null;

                if (package.Parameters.TryGetValue("version", out var version))
                {
                    requestedVersion = version.First();
                }

                if (requestedVersion == null)
                {
                    _log.Warning("Tool {0} is already installed, and no specific version has been requested via pre-processor directive, so leaving current version installed.", package.Package);
                }
                else if (requestedVersion.ToLowerInvariant() != installedTool.Version.ToLowerInvariant())
                {
                    _log.Warning("Tool {0} is already installed, but a different version has been requested.  Uninstall/install will now be performed...", package.Package);
                    RunDotNetTool(package, toolsFolderDirectoryPath, DotNetToolOperation.Uninstall);
                    RunDotNetTool(package, toolsFolderDirectoryPath, DotNetToolOperation.Install);
                }
                else
                {
                    _log.Information("Tool {0} is already installed, with required version.", package.Package);
                }
            }
            else
            {
                // The tool isn't already installed, go ahead and install it
                RunDotNetTool(package, toolsFolderDirectoryPath, DotNetToolOperation.Install);
            }

            var result = _contentResolver.GetFiles(package, type);
            if (result.Count == 0)
            {
                const string format = "Could not find any relevant files for tool '{0}'. Perhaps you need an include parameter?";
                _log.Warning(format, package.Package);
            }

            return result;
        }

        private List<DotNetToolPackage> GetInstalledTools(string toolLocation)
        {
            var toolLocationArgument = string.Empty;
            if (toolLocation == "global")
            {
                toolLocationArgument = "--global";
            }
            else if (toolLocation == "local")
            {
                toolLocationArgument = "--local";
            }
            else
            {
                toolLocationArgument = string.Format("--tool-path \"{0}\"", toolLocation);
                var toolLocationDirectoryPath = new DirectoryPath(toolLocation).MakeAbsolute(_environment);
                var toolLocationDirectory = _fileSystem.GetDirectory(toolLocationDirectoryPath);

                // If the requested tools path doesn't exist, then there can't be any tools
                // installed there, so simply return an empty list.
                if (!toolLocationDirectory.Exists)
                {
                    _log.Debug("Specified installation location doesn't currently exist.");
                    return new List<DotNetToolPackage>();
                }
            }

            var isInstalledProcess = _processRunner.Start(
                "dotnet",
                new ProcessSettings
                {
                    Arguments = string.Concat("tool list ", toolLocationArgument),
                    RedirectStandardOutput = true,
                    Silent = _log.Verbosity < Verbosity.Diagnostic
                });

            isInstalledProcess.WaitForExit();

            var installedTools = isInstalledProcess.GetStandardOutput().ToList();
            var installedToolNames = new List<DotNetToolPackage>();

            const string pattern = @"(?<packageName>[^\s]+)\s+(?<packageVersion>[^\s]+)\s+(?<packageShortCode>[^\s]+)(?:\s+(?<packageManifest>[^\s]+))?";

            foreach (var installedTool in installedTools.Skip(2))
            {
                foreach (Match match in Regex.Matches(installedTool, pattern, RegexOptions.IgnoreCase))
                {
                    _log.Debug("Adding tool {0}", match.Groups["packageName"].Value);
                    installedToolNames.Add(new DotNetToolPackage
                    {
                        Id = match.Groups["packageName"].Value,
                        Version = match.Groups["packageVersion"].Value,
                        ShortCode = match.Groups["packageShortCode"].Value,
                        Manifest = match.Groups["packageManifest"].Value
                    });
                }
            }

            _log.Debug("There are {0} dotnet tools installed", installedToolNames.Count);
            return installedToolNames;
        }

        private void RunDotNetTool(PackageReference package, DirectoryPath toolsFolderDirectoryPath, DotNetToolOperation operation)
        {
            // Install the tool....
            _log.Debug("Running dotnet tool with operation {0}: {1}...", operation, package.Package);
            var process = _processRunner.Start(
                "dotnet",
                new ProcessSettings
                {
                    Arguments = GetArguments(package, operation, _log, toolsFolderDirectoryPath),
                    RedirectStandardOutput = true,
                    Silent = _log.Verbosity < Verbosity.Diagnostic,
                    NoWorkingDirectory = true
                });

            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                _log.Warning("dotnet exited with {0}", exitCode);
                var output = string.Join(Environment.NewLine, process.GetStandardError());
                _log.Verbose(Verbosity.Diagnostic, "Output:{0}{1}", Environment.NewLine, output);
            }
        }

        private static ProcessArgumentBuilder GetArguments(
            PackageReference definition,
            DotNetToolOperation operation,
            ICakeLog log,
            DirectoryPath toolDirectoryPath)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("tool");
            arguments.Append(Enum.GetName(operation).ToLowerInvariant());
            arguments.AppendQuoted(definition.Package);

            if (definition.Parameters.ContainsKey("global"))
            {
                arguments.Append("--global");
            }
            else if (definition.Parameters.ContainsKey("local"))
            {
                arguments.Append("--local");
            }
            else
            {
                arguments.Append("--tool-path");
                arguments.AppendQuoted(toolDirectoryPath.FullPath);
            }

            // Tool manifest
            if (definition.Parameters.ContainsKey("tool-manifest"))
            {
                arguments.Append("--tool-manifest");
                arguments.AppendQuoted(definition.Parameters["tool-manifest"].First());
            }

            if (operation != DotNetToolOperation.Uninstall)
            {
                if (definition.Address != null)
                {
                    arguments.Append("--add-source");
                    arguments.AppendQuoted(definition.Address.AbsoluteUri);
                }

                // Version
                if (definition.Parameters.TryGetValue("version", out var version))
                {
                    arguments.Append("--version");
                    arguments.Append(version.First());
                }

                // Config File
                if (definition.Parameters.TryGetValue("configfile", out var config))
                {
                    arguments.Append("--configfile");
                    arguments.AppendQuoted(config.First());
                }

                // Whether to ignore failed sources
                if (definition.Parameters.ContainsKey("ignore-failed-sources"))
                {
                    arguments.Append("--ignore-failed-sources");
                }

                // Framework
                if (definition.Parameters.TryGetValue("framework", out var framework))
                {
                    arguments.Append("--framework");
                    arguments.Append(framework.First());
                }

                switch (log.Verbosity)
                {
                    case Verbosity.Quiet:
                        arguments.Append("--verbosity");
                        arguments.Append("quiet");
                        break;
                    case Verbosity.Minimal:
                        arguments.Append("--verbosity");
                        arguments.Append("minimal");
                        break;
                    case Verbosity.Normal:
                        arguments.Append("--verbosity");
                        arguments.Append("normal");
                        break;
                    case Verbosity.Verbose:
                        arguments.Append("--verbosity");
                        arguments.Append("detailed");
                        break;
                    case Verbosity.Diagnostic:
                        arguments.Append("--verbosity");
                        arguments.Append("diagnostic");
                        break;
                    default:
                        arguments.Append("--verbosity");
                        arguments.Append("normal");
                        break;
                }
            }

            return arguments;
        }
    }
}
