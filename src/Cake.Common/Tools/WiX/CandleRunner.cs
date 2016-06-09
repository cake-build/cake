// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Candle runner.
    /// </summary>
    public sealed class CandleRunner : Tool<CandleSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public CandleRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _environment = environment;
        }

        /// <summary>
        /// Runs Candle with the specified source files and settings.
        /// </summary>
        /// <param name="sourceFiles">The source files (<c>.wxs</c>) to compile.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> sourceFiles, CandleSettings settings)
        {
            if (sourceFiles == null)
            {
                throw new ArgumentNullException("sourceFiles");
            }

            var sourceFilesArray = sourceFiles as FilePath[] ?? sourceFiles.ToArray();
            if (!sourceFilesArray.Any())
            {
                throw new ArgumentException("No source files specified.", "sourceFiles");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(sourceFilesArray, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> sourceFiles, CandleSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Architecture
            if (settings.Architecture.HasValue)
            {
                builder.Append("-arch");
                builder.Append(GetArchitectureName(settings.Architecture.Value));
            }

            // Add defines
            if (settings.Defines != null && settings.Defines.Any())
            {
                var defines = settings.Defines.Select(define => string.Format(CultureInfo.InvariantCulture, "-d{0}={1}", define.Key, define.Value));
                foreach (var define in defines)
                {
                    builder.Append(define);
                }
            }

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                var extensions = settings.Extensions.Select(extension => string.Format(CultureInfo.InvariantCulture, "-ext {0}", extension));
                foreach (var extension in extensions)
                {
                    builder.Append(extension);
                }
            }

            // FIPS
            if (settings.FIPS)
            {
                builder.Append("-fips");
            }

            // No logo
            if (settings.NoLogo)
            {
                builder.Append("-nologo");
            }

            // Output directory
            if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullPath))
            {
                // Candle want the path to end with \\, double separator chars.
                var fullPath = string.Concat(settings.OutputDirectory.MakeAbsolute(_environment).FullPath, '\\', '\\');

                builder.Append("-o");
                builder.AppendQuoted(fullPath);
            }

            // Pedantic
            if (settings.Pedantic)
            {
                builder.Append("-pedantic");
            }

            // Show source trace
            if (settings.ShowSourceTrace)
            {
                builder.Append("-trace");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Source files (.wxs)
            foreach (var sourceFile in sourceFiles.Select(file => file.MakeAbsolute(_environment).FullPath))
            {
                builder.AppendQuoted(sourceFile);
            }

            return builder;
        }

        private static string GetArchitectureName(Architecture arch)
        {
            switch (arch)
            {
                case Architecture.IA64:
                    return "ia64";
                case Architecture.X64:
                    return "x64";
                case Architecture.X86:
                    return "x86";
                default:
                    throw new NotSupportedException("The provided architecture is not valid.");
            }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Candle";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "candle.exe" };
        }
    }
}
