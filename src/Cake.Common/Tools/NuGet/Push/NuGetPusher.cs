using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Push
{
    /// <summary>
    /// The NuGet package pusher.
    /// </summary>
    public sealed class NuGetPusher
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPusher"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetPusher(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Pushes a NuGet package to a NuGet server and publishes it.
        /// </summary>
        /// <param name="packageFilePath">The package file path.</param>
        /// <param name="settings">The settings.</param>
        public void Push(FilePath packageFilePath, NuGetPushSettings settings)
        {
            if (packageFilePath == null)
            {
                throw new ArgumentNullException("packageFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Find the NuGet executable.
            var toolPath = NuGetResolver.GetToolPath(_environment, _globber, settings.ToolPath);

            // Start the process.
            var processInfo = GetProcessStartInfo(toolPath, packageFilePath, settings);
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

        private ProcessStartInfo GetProcessStartInfo(FilePath toolPath, FilePath packageFilePath, NuGetPushSettings settings)
        {
            return NuGetResolver.GetProcessStartInfo(_environment, toolPath,
                () => GetPackParameters(packageFilePath, settings));
        }

        private ICollection<string> GetPackParameters(FilePath packageFilePath, NuGetPushSettings settings)
        {
            var parameters = new List<string> { "push" };

            parameters.Add(packageFilePath.MakeAbsolute(_environment).FullPath.Quote());

            if (settings.ApiKey != null)
            {
                parameters.Add(settings.ApiKey);
            }

            if (settings.NonInteractive)
            {
                parameters.Add("-NonInteractive");
            }

            if (settings.ConfigFile != null)
            {
                parameters.Add("-ConfigFile");
                parameters.Add(settings.ConfigFile.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.Source != null)
            {
                parameters.Add("-Source");
                parameters.Add(settings.Source.Quote());
            }

            if (settings.Timeout != null)
            {
                parameters.Add("-Timeout");
                parameters.Add(Convert.ToInt32(settings.Timeout.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture));
            }

            if (settings.Verbosity != null)
            {
                parameters.Add("-Verbosity");
                parameters.Add(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            return parameters;
        }
    }
}
