using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Candle runner.
    /// </summary>
    public sealed class CandleRunner : Tool<CandleSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public CandleRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Runs Candle with the specified source files and settings.
        /// </summary>
        /// <param name="sourceFiles">The source files (.wxs) to compile.</param>
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

            Run(settings, GetArguments(sourceFilesArray, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(IEnumerable<FilePath> sourceFiles, CandleSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            // Architecture
            if (settings.Architecture.HasValue)
            {
                builder.AppendText("-arch");
                builder.AppendText(GetArchitectureName(settings.Architecture.Value));
            }

            // Add defines
            if (settings.Defines != null && settings.Defines.Any())
            {
                var defines = settings.Defines.Select(define => string.Format(CultureInfo.InvariantCulture, "-d{0}={1}", define.Key, define.Value));
                foreach (var define in defines)
                {
                    builder.AppendText(define);
                }
            }

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                var extensions = settings.Extensions.Select(extension => string.Format(CultureInfo.InvariantCulture, "-ext {0}", extension));
                foreach (var extension in extensions)
                {
                    builder.AppendText(extension);
                }
            }

            // FIPS
            if (settings.FIPS)
            {
                builder.AppendText("-fips");
            }

            // No logo
            if (settings.NoLogo)
            {
                builder.AppendText("-nologo");
            }

            // Output directory
            if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullPath))
            {
                // Candle want the path to end with \\, double separator chars.
                var separatorChar = System.IO.Path.DirectorySeparatorChar;
                var fullPath = string.Concat(settings.OutputDirectory.MakeAbsolute(_environment).FullPath, separatorChar, separatorChar);

                builder.AppendText("-o");
                builder.AppendQuotedText(fullPath);
            }

            // Pedantic
            if (settings.Pedantic)
            {
                builder.AppendText("-pedantic");
            }

            // Show source trace
            if (settings.ShowSourceTrace)
            {
                builder.AppendText("-trace");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.AppendText("-v");
            }

            // Source files (.wxs)
            foreach (var sourceFile in sourceFiles.Select(file => file.MakeAbsolute(_environment).FullPath))
            {
                builder.AppendQuotedText(sourceFile);
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
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(CandleSettings settings)
        {
            const string expression = "./tools/**/candle.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
