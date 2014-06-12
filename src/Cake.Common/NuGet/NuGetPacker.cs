using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.Common.NuGet
{
    public sealed class NuGetPacker
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        public NuGetPacker(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        public void Pack(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            if (nuspecFilePath == null)
            {
                throw new ArgumentNullException("nuspecFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Find the NuGet executable.
            var query = string.Format("./tools/**/NuGet.exe");
            var nugetExePath = _globber.GetFiles(query).FirstOrDefault();
            if (nugetExePath == null)
            {
                throw new CakeException("Could not find NuGet.exe.");
            }

            // Start the process.
            var processInfo = GetProcessStartInfo(nugetExePath, nuspecFilePath, settings);
            var process = _processRunner.Start(processInfo);
            if (process == null)
            {
                throw new CakeException("NuGet.exe was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("NuGet packager failed.");
            }
        }

        private ProcessStartInfo GetProcessStartInfo(FilePath nugetExePath, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var parameters = new List<string> { "pack" };

            // Version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                parameters.Add("-Version");
                parameters.Add(settings.Version.Quote());
            }

            // Base path
            if (settings.BasePath != null)
            {
                parameters.Add("-BasePath");
                parameters.Add(settings.BasePath.FullPath.Quote());                
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                parameters.Add("-OutputDirectory");
                parameters.Add(settings.OutputDirectory.FullPath.Quote());                
            }

            // Nuspec file
            parameters.Add(nuspecFilePath.FullPath.Quote());

            // No package analysis?
            if (settings.NoPackageAnalysis)
            {
                parameters.Add("-NoPackageAnalysis");
            }

            // Symbols?
            if (settings.Symbols)
            {
                parameters.Add("-Symbols");
            }

            return new ProcessStartInfo(nugetExePath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
