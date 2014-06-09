using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.MSBuild
{
    public sealed class MSBuildRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _runner;

        public MSBuildRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner runner = null)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _runner = runner ?? new ProcessRunner();
        }

        public void Run(MSBuildSettings settings)
        {
            // Get the MSBuild path.
            var msBuildPath = MSBuildResolver.GetMSBuildPath(_environment, settings.ToolVersion, settings.PlatformTarget);
            if (!_fileSystem.GetFile(msBuildPath).Exists)
            {
                var message = string.Format("Could not find MSBuild at {0}", msBuildPath);
                throw new CakeException(message);
            }

            // Start the process.
            var processInfo = GetProcessStartInfo(settings, msBuildPath);
            var process = _runner.Start(processInfo);
            if (process == null)
            {
                throw new CakeException("MSBuild process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Build failed.");
            }
        }

        private ProcessStartInfo GetProcessStartInfo(MSBuildSettings settings, FilePath msBuildPath)
        {
            var parameters = new List<string>();
            var properties = new List<string>();

            // Got a specific configuration in mind?
            if (!string.IsNullOrWhiteSpace(settings.Configuration))
            {
                // Add the configuration as a property.
                var configuration = settings.Configuration;
                properties.Add(string.Concat("Configuration", "=", configuration));
            }

            // Got any properties?
            if (settings.Properties.Count > 0)
            {
                properties.AddRange(settings.Properties.Select(x => string.Concat(x.Key, "=", x.Value)));
            }
            if (properties.Count > 0)
            {
                parameters.Add(string.Concat("/property:", string.Join(";", properties)));
            }

            // Got any targets?
            if (settings.Targets.Count > 0)
            {
                var targets = string.Join(";", settings.Targets);
                parameters.Add(string.Concat("/target:", targets));
            }
            else
            {
                // Use default target.
                parameters.Add("/target:Build");
            }

            // Add the solution as the last parameter.
            parameters.Add(settings.Solution.FullPath);

            return new ProcessStartInfo(msBuildPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
