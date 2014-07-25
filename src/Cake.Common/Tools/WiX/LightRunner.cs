using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Light runner.
    /// </summary>
    public sealed class LightRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightRunner"/> class.
        /// </summary>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public LightRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            if (environment == null) throw new ArgumentNullException("environment");
            if (globber == null) throw new ArgumentNullException("globber");
            if (processRunner == null) throw new ArgumentNullException("processRunner");

            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Runs Light with the specified input object files and settings.
        /// </summary>
        /// <param name="objectFiles">The object files (.wixobj).</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> objectFiles, LightSettings settings)
        {
            if (objectFiles == null)
            {
                throw new ArgumentNullException("objectFiles");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var objectFilesArray = objectFiles as FilePath[] ?? objectFiles.ToArray();
            if (!objectFilesArray.Any())
            {
                throw new ArgumentException("No object files provided.", "objectFiles");
            }

            // Find light.exe
            var toolPath = GetToolPath(settings);

            // Get process start info
            var info = GetProcessStartInfo(objectFilesArray, settings, toolPath);

            // Run the process
            var process = _processRunner.Start(info);
            if (process == null)
            {
                throw new CakeException("Light process was not started.");
            }

            // Wait for exit
            process.WaitForExit();

            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failed to run Light.");
            }
        }

        private FilePath GetToolPath(LightSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }

            var expression = string.Format("./tools/**/light.exe");
            var runnerPath = _globber.GetFiles(expression).FirstOrDefault();

            if (runnerPath == null)
            {
                throw new CakeException("Could not find light.exe.");
            }

            return runnerPath;
        }

        private ProcessStartInfo GetProcessStartInfo(IEnumerable<FilePath> objectFiles, LightSettings settings, FilePath toolPath)
        {
            var parameters = new List<string>();

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

            // No logo
            if (settings.NoLogo)
            {
                parameters.Add("-nologo");
            }

            // Output file
            if (settings.OutputFile != null && !string.IsNullOrEmpty(settings.OutputFile.FullPath))
            {
                parameters.Add(string.Format("-o {0}", settings.OutputFile.MakeAbsolute(_environment).FullPath.Quote()));
            }

            // Raw arguments
            if (!string.IsNullOrEmpty(settings.RawArguments))
            {
                parameters.Add(settings.RawArguments);
            }

            // Object files (.wixobj)
            parameters.AddRange(objectFiles.Select(file => string.Format(file.MakeAbsolute(_environment).FullPath.Quote())));

            return new ProcessStartInfo(toolPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
