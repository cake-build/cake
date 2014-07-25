using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Candle runner.
    /// </summary>
    public sealed class CandleRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleRunner"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public CandleRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            if (environment == null) throw new ArgumentNullException("environment");
            if (globber == null) throw new ArgumentNullException("globber");
            if (processRunner == null) throw new ArgumentNullException("processRunner");

            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
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

            // Find candle.exe
            var toolPath = GetToolPath(settings);

            // Get process start info
            var info = GetProcessStartInfo(sourceFilesArray, settings, toolPath);

            // Run the process
            var process = _processRunner.Start(info);
            if (process == null)
            {
                throw new CakeException("Candle process was not started.");
            }

            // Wait for exit
            process.WaitForExit();

            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failed to run Candle.");
            }
        }

        private FilePath GetToolPath(CandleSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }

            var expression = string.Format("./tools/**/candle.exe");
            var runnerPath = _globber.GetFiles(expression).FirstOrDefault();

            if (runnerPath == null)
            {
                throw new CakeException("Could not find candle.exe.");
            }

            return runnerPath;
        }

        private ProcessStartInfo GetProcessStartInfo(IEnumerable<FilePath> sourceFiles, CandleSettings settings, FilePath toolPath)
        {
            var parameters = new List<string>();

            // Architecture
            if (settings.Architecture.HasValue)
            {
                parameters.Add(string.Format("-arch {0}", GetArchitectureName(settings.Architecture.Value)));
            }

            // Add defines
            if (settings.Defines != null && settings.Defines.Any())
            {
                parameters.AddRange(settings.Defines.Select(define => string.Format("-d{0}={1}", define.Key, define.Value)));
            }

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                parameters.AddRange(settings.Extensions.Select(extension => string.Format("-ext {0}", extension)));
            }

            // FIPS
            if (settings.FIPS)
            {
                parameters.Add("-fips");
            }

            // No logo
            if (settings.NoLogo)
            {
                parameters.Add("-nologo");
            }

            // Output directory
            if (settings.OutputDirectory != null && !string.IsNullOrEmpty(settings.OutputDirectory.FullPath))
            {
                // Candle want the path to end with \\, double separator chars.
                var separatorChar = System.IO.Path.DirectorySeparatorChar;
                var fullPath = string.Concat(settings.OutputDirectory.MakeAbsolute(_environment).FullPath, separatorChar, separatorChar);

                parameters.Add(string.Format("-o {0}", fullPath.Quote()));
            }

            // Pedantic
            if (settings.Pedantic)
            {
                parameters.Add("-pedantic");
            }

            // Show source trace
            if (settings.ShowSourceTrace)
            {
                parameters.Add("-trace");
            }

            // Verbose
            if (settings.Verbose)
            {
                parameters.Add("-v");
            }

            // Source files (.wxs)
            parameters.AddRange(sourceFiles.Select(file => string.Format(file.MakeAbsolute(_environment).FullPath.Quote())));

            return new ProcessStartInfo(toolPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
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
    }
}
