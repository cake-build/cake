using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public sealed class NuGetRestorer
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        public NuGetRestorer(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        public void Restore(NuGetRestoreSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Find the NuGet executable.
            var toolPath = NuGetResolver.GetToolPath(_environment, _globber, settings.ToolPath);

            // Start the process.
            var processInfo = NuGetResolver.GetProcessStartInfo(_environment, toolPath, ()=>GetPackParameters(settings));
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

        private static ICollection<string> GetPackParameters(NuGetRestoreSettings settings)
        {
            var parameters = new List<string>
            {
                "restore",
                settings.Solution.FullPath.Quote()
            };

            // RequireConsent?
            if (settings.RequireConsent)
            {
                parameters.Add("-RequireConsent");
            }
          
            // Packages Directory
            if (settings.PackagesDirectory != null)
            {
                parameters.Add("-PackagesDirectory");
                parameters.Add(settings.PackagesDirectory.FullPath.Quote());
            }

            // List of package sources
            if (settings.Source != null && settings.Source.Count>0)
            {
                parameters.Add("-Source");
                parameters.Add(string.Join(";", settings.Source).Quote());
            }
           
            // No Cache?
            if (settings.NoCache)
            {
                parameters.Add("-NoCache");
            }

            // Disable Parallel Processing?
            if (settings.DisableParallelProcessing)
            {
                parameters.Add("-DisableParallelProcessing");
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                parameters.Add("-Verbosity");
                parameters.Add(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }
          
            // Packages Directory
            if (settings.ConfigFile != null)
            {
                parameters.Add("-ConfigFile");
                parameters.Add(settings.ConfigFile.FullPath.Quote());
            }

            parameters.Add("-NonInteractive");

            return parameters;
        }
    }
}
