// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere
{
    /// <summary>
    /// Base class for all VSWhere related tools.
    /// Used to locate Visual Studio.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class VSWhereTool<TSettings> : Tool<TSettings>
        where TSettings : ToolSettings
    {
        private const string VSWhereExecutableName = "vswhere.exe";
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolLocator">The tool locator service.</param>
        protected VSWhereTool(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator toolLocator)
            : base(fileSystem, environment, processRunner, toolLocator)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "VSWhere";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { VSWhereExecutableName };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            /*
             * According to https://blogs.msdn.microsoft.com/heaths/2017/04/21/vswhere-is-now-installed-with-visual-studio-2017/,
             * Starting in the latest preview release of Visual Studio version 15.2 (26418.1-Preview), you can now find vswhere installed in
             * “%ProgramFiles(x86)%\Microsoft Visual Studio\Installer” (on 32-bit operating systems before Windows 10, you should use
             * “%ProgramFiles%\Microsoft Visual Studio\Installer”).
             */

            return new FilePath[]
                {
                    _environment.GetSpecialPath(_environment.Platform.Is64Bit ? SpecialPath.ProgramFilesX86 : SpecialPath.ProgramFiles).CombineWithFilePath("Microsoft Visual Studio/Installer/" + VSWhereExecutableName),
                };
        }

        /// <summary>
        /// Runs VSWhere with supplied arguments and parses installation paths.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The parsed file paths.</returns>
        protected DirectoryPathCollection RunVSWhere(TSettings settings, ProcessArgumentBuilder builder)
        {
            IEnumerable<string> installationPaths = null;
            Run(settings, builder, new ProcessSettings { RedirectStandardOutput = true },
                process => installationPaths = process.GetStandardOutput());

            return new DirectoryPathCollection(installationPaths?.Select(DirectoryPath.FromString) ?? Enumerable.Empty<DirectoryPath>());
        }

        /// <summary>
        /// Adds common arguments to the process builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The process argument builder.</param>
        /// <returns>The process argument builder.</returns>
        protected ProcessArgumentBuilder AddCommonArguments(VSWhereSettings settings, ProcessArgumentBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("-version");
                builder.AppendQuoted(settings.Version);
            }

            if (!string.IsNullOrWhiteSpace(settings.Requires))
            {
                builder.Append("-requires");
                builder.Append(settings.Requires);
            }

            if (!string.IsNullOrWhiteSpace(settings.ReturnProperty))
            {
                builder.Append("-property");
                builder.Append(settings.ReturnProperty);
            }

            if (settings.IncludePrerelease)
            {
                builder.Append("-prerelease");
            }

            builder.Append("-nologo");

            return builder;
        }
    }
}
