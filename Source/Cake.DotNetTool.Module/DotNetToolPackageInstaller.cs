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
        /// <param name="config">the configuration</param>
        /// <param name="fileSystem">The file system.</param>
        public DotNetToolPackageInstaller(ICakeEnvironment environment, IProcessRunner processRunner, ICakeLog log, IDotNetToolContentResolver contentResolver, ICakeConfiguration config, IFileSystem fileSystem)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            if (contentResolver == null)
            {
                throw new ArgumentNullException(nameof(contentResolver));
            }

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
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

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
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // We are going to assume that the default install location is the
            // currently configured location for the Cake Tools Folder
            var toolsFolderDirectoryPath = _config.GetToolPath(_environment.WorkingDirectory, _environment);
            _log.Information("Configured Tools Folder: {0}", toolsFolderDirectoryPath);

            var toolLocation = toolsFolderDirectoryPath.FullPath;
            if(package.Parameters.ContainsKey("global"))
            {
                toolLocation = "global";
            }

            // First we need to check if the Tool is already installed
            var installedToolNames = GetInstalledTools(toolLocation);

            _log.Debug("Checking for tool: {0}", package.Package.ToLowerInvariant());
            if(installedToolNames.Contains(package.Package.ToLowerInvariant()))
            {
                _log.Information("Tool {0} is already installed, so nothing to do here.", package.Package);
            }
            else
            {
                InstallTool(package, toolsFolderDirectoryPath);
            }

            var result = _contentResolver.GetFiles(package, type);
            if (result.Count == 0)
            {
                const string format = "Could not find any relevant files for tool '{0}'. Perhaps you need an include parameter?";
                _log.Warning(format, package.Package);
            }

            return result;
        }

        private List<string> GetInstalledTools(string toolLocation)
        {
            var toolLocationArgument = string.Empty;
            if(toolLocation != "global")
            {
                toolLocationArgument = string.Format("--tool-path \"{0}\"", toolLocation);
                var toolLocationDirectoryPath = new DirectoryPath(toolLocation).MakeAbsolute(_environment);
                var toolLocationDirectory = _fileSystem.GetDirectory(toolLocationDirectoryPath);

                // If the requested tools path doesn't exist, then there can't be any tools
                // installed there, so simply return an empty list.
                if(!toolLocationDirectory.Exists)
                {
                    _log.Information("Specified installation location doesn't currently exist.");
                    return new List<string>();
                }
            }
            else
            {
                toolLocationArgument = "--global";
            }

            var isInstalledProcess = _processRunner.Start(
                "dotnet",
                new ProcessSettings {
                    Arguments = string.Format("tool list {0}", toolLocationArgument),
                    RedirectStandardOutput = true,
                    Silent = _log.Verbosity < Verbosity.Diagnostic });

            isInstalledProcess.WaitForExit();

            var installedTools = isInstalledProcess.GetStandardOutput().ToList();
            var installedToolNames = new List<string>();

            string pattern = @"(?<packageName>[^\s]+)\s+(?<packageVersion>[^\s]+)\s+(?<packageShortCode>[^`s])";

            foreach(var installedTool in installedTools.Skip(2))
            {
                foreach (Match match in Regex.Matches(installedTool, pattern, RegexOptions.IgnoreCase))
                {
                    _log.Debug("Adding tool {0}", match.Groups["packageName"].Value);
                    installedToolNames.Add(match.Groups["packageName"].Value);
                }
            }

            _log.Debug("There are {0} dotnet tools installed", installedToolNames.Count);
            return installedToolNames;
        }

        private void InstallTool(PackageReference package, DirectoryPath toolsFolderDirectoryPath)
        {
            // Install the tool....
            _log.Debug("Installing dotnet tool: {0}...", package.Package);
            var process = _processRunner.Start(
                "dotnet",
                new ProcessSettings {
                    Arguments = GetArguments(package, _log, toolsFolderDirectoryPath),
                    RedirectStandardOutput = true,
                    Silent = _log.Verbosity < Verbosity.Diagnostic,
                    NoWorkingDirectory = true });

            process.WaitForExit();

            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                _log.Warning("dotnet exited with {0}", exitCode);
                var output = string.Join(Environment.NewLine, process.GetStandardError());
                _log.Verbose(Verbosity.Diagnostic, "Output:\r\n{0}", output);
            }
        }
        private static ProcessArgumentBuilder GetArguments(
            PackageReference definition,
            ICakeLog log,
            DirectoryPath toolDirectoryPath)
        {
            var arguments = new ProcessArgumentBuilder();

            arguments.Append("tool install");
            arguments.AppendQuoted(definition.Package);

            if(definition.Parameters.ContainsKey("global"))
            {
                arguments.Append("--global");
            }
            else
            {
                arguments.Append("--tool-path");
                arguments.AppendQuoted(toolDirectoryPath.FullPath);
            }

            if (definition.Address != null)
            {
                arguments.Append("--add-source");
                arguments.AppendQuoted(definition.Address.AbsoluteUri);
            }

            // Version
            if (definition.Parameters.ContainsKey("version"))
            {
                arguments.Append("--version");
                arguments.Append(definition.Parameters["version"].First());
            }

            // Config File
            if(definition.Parameters.ContainsKey("configfile"))
            {
                arguments.Append("--config-file");
                arguments.AppendQuoted(definition.Parameters["configfile"].First());
            }

            // Framework
            if (definition.Parameters.ContainsKey("framework"))
            {
                arguments.Append("--framework");
                arguments.Append(definition.Parameters["framework"].First());
            }

            switch(log.Verbosity)
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

            return arguments;
        }
    }
}
