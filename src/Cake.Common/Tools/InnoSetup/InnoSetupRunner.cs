// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.InnoSetup
{
    /// <summary>
    /// The runner which executes Inno Setup.
    /// </summary>
    public sealed class InnoSetupRunner : Tool<InnoSetupSettings>
    {
        private readonly IRegistry _registry;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="InnoSetupRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public InnoSetupRunner(
            IFileSystem fileSystem,
            IRegistry registry,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _registry = registry;
            _environment = environment;
        }

        /// <summary>
        /// Runs <c>iscc.exe</c> with the specified script files and settings.
        /// </summary>
        /// <param name="scriptFile">The script file (<c>.iss</c>) to compile.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath scriptFile, InnoSetupSettings settings)
        {
            if (scriptFile == null)
            {
                throw new ArgumentNullException(nameof(scriptFile));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            Run(settings, GetArguments(scriptFile, settings));
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "InnoSetup";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "iscc.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(InnoSetupSettings settings)
        {
            foreach (var keyPath in GetAlternativeRegistryKeyPathsForVersion(settings.Version))
            {
                using (var innoSetupKey = _registry.LocalMachine.OpenKey(keyPath))
                {
                    var installationPath = innoSetupKey?.GetValue("InstallLocation") as string;
                    if (!string.IsNullOrEmpty(installationPath))
                    {
                        var directory = new DirectoryPath(installationPath);
                        var isccPath = directory.CombineWithFilePath("iscc.exe");
                        return new[] { isccPath };
                    }
                }
            }

            return base.GetAlternativeToolPaths(settings);
        }

        private IEnumerable<string> GetAlternativeRegistryKeyPathsForVersion(InnoSetupVersion? version)
        {
            if (version != null)
            {
                return new[] { GetRegistryKeyPathForVersion(version.Value) };
            }

            var versionsToConsider = new[]
            {
                InnoSetupVersion.InnoSetup6,
                InnoSetupVersion.InnoSetup5
            };
            return versionsToConsider.Select(GetRegistryKeyPathForVersion);
        }

        private string GetRegistryKeyPathForVersion(InnoSetupVersion version)
        {
            // On 64-bit Windows, the registry key for Inno Setup will be accessible under Wow6432Node
            var softwareKeyPath = _environment.Platform.Is64Bit ? @"SOFTWARE\Wow6432Node\" : @"SOFTWARE\";
            switch (version)
            {
                case InnoSetupVersion.InnoSetup6:
                    return $@"{softwareKeyPath}Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 6_is1";
                case InnoSetupVersion.InnoSetup5:
                    return $@"{softwareKeyPath}Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 5_is1";
                default:
                    throw new ArgumentOutOfRangeException(nameof(version), version, "Missing switch case");
            }
        }

        private ProcessArgumentBuilder GetArguments(FilePath scriptFile, InnoSetupSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Defines (/Ddefine[=value]
            if (settings.Defines != null)
            {
                foreach (var item in settings.Defines)
                {
                    builder.Append(string.IsNullOrEmpty(item.Value)
                        ? string.Format(CultureInfo.InvariantCulture, "/D{0}", item.Key)
                        : string.Format(CultureInfo.InvariantCulture, "/D{0}={1}", item.Key,
                            new QuotedArgument(new TextArgument(item.Value))));
                }
            }

            // Enable output
            if (settings.EnableOutput.HasValue)
            {
                builder.Append(settings.EnableOutput.Value ? "/O+" : "/O-");
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.AppendSwitchQuoted("/O", string.Empty, settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Output base file name
            if (!string.IsNullOrEmpty(settings.OutputBaseFilename))
            {
                builder.AppendSwitchQuoted("/F", string.Empty, settings.OutputBaseFilename);
            }

            // Quiet mode
            switch (settings.QuietMode)
            {
                case InnoSetupQuietMode.Off:
                    break;
                case InnoSetupQuietMode.Quiet:
                    builder.Append("/Q");
                    break;
                case InnoSetupQuietMode.QuietWithProgress:
                    builder.Append("/Qp");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Quoted Script file
            builder.AppendQuoted(scriptFile.MakeAbsolute(_environment).FullPath);

            return builder;
        }
    }
}