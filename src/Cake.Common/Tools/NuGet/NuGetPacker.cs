using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// The NuGet packer.
    /// </summary>
    public sealed class NuGetPacker
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPacker"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetPacker(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Creates a NuGet package from the specified Nuspec file.
        /// </summary>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
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
            var toolPath = NuGetResolver.GetToolPath(_environment, _globber, settings.ToolPath);

            // Start the process.
            var processInfo = NuGetResolver.GetProcessStartInfo(_environment, toolPath, ()=>GetPackParameters(nuspecFilePath, settings));
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

        private ICollection<string> GetPackParameters(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var parameters = new List<string> {"pack"};

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
                parameters.Add(settings.BasePath.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                parameters.Add("-OutputDirectory");
                parameters.Add(settings.OutputDirectory.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Nuspec file
            parameters.Add(nuspecFilePath.MakeAbsolute(_environment).FullPath.Quote());

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
            return parameters;
        }
    }
}
