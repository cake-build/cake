// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Common.Net;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Responsible for communicating with AppVeyor.
    /// </summary>
    public sealed class AppVeyorProvider : IAppVeyorProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _log;

        /// <inheritdoc/>
        public bool IsRunningOnAppVeyor => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("APPVEYOR"));

        /// <inheritdoc/>
        public AppVeyorEnvironmentInfo Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The cake log.</param>
        public AppVeyorProvider(ICakeEnvironment environment, IProcessRunner processRunner, ICakeLog log)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            Environment = new AppVeyorEnvironmentInfo(environment);
        }

        /// <inheritdoc/>
        public void UploadArtifact(FilePath path)
        {
            UploadArtifact(path, settings => settings.SetArtifactType(AppVeyorUploadArtifactType.Auto));
        }

        /// <inheritdoc/>
        public void UploadArtifact(FilePath path, AppVeyorUploadArtifactsSettings settings)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Make path absolute.
            path = path.IsRelative ? path.MakeAbsolute(_environment) : path;

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("PushArtifact");
            arguments.AppendQuoted(path.FullPath);
            arguments.Append("-Type");
            arguments.Append(settings.ArtifactType.ToString());
            if (!string.IsNullOrEmpty(settings.DeploymentName))
            {
                if (settings.DeploymentName.Contains(" "))
                {
                    throw new CakeException("The deployment name can not contain spaces");
                }
                arguments.Append("-DeploymentName");
                arguments.AppendQuoted(settings.DeploymentName);
            }

            StartAppVeyor(arguments);
        }

        /// <inheritdoc/>
        public void UploadArtifact(FilePath path, Action<AppVeyorUploadArtifactsSettings> settingsAction)
        {
            if (settingsAction == null)
            {
                throw new ArgumentNullException(nameof(settingsAction));
            }

            var settings = new AppVeyorUploadArtifactsSettings();
            settingsAction(settings);
            UploadArtifact(path, settings);
        }

        /// <inheritdoc/>
        public void UploadTestResults(FilePath path, AppVeyorTestResultsType resultsType)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            var baseUri = _environment.GetEnvironmentVariable("APPVEYOR_URL").TrimEnd('/');

            if (string.IsNullOrWhiteSpace(baseUri))
            {
                throw new CakeException("Failed to get AppVeyor API url.");
            }

            var url = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}/api/testresults/{1}/{2}", baseUri, resultsType, Environment.JobId).ToLowerInvariant());

            _log.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Uploading [{0}] to [{1}]", path.FullPath, url);
            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.UploadFileAsync(url, path.FullPath, "text/xml");
                    var content = await response.Content.ReadAsStringAsync();
                    _log.Write(Verbosity.Diagnostic, LogLevel.Verbose, "Server response [{0}:{1}]:\n\r{2}", response.StatusCode, response.ReasonPhrase, content);
                }
            }).Wait();
        }

        /// <inheritdoc/>
        public void UpdateBuildVersion(string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new CakeException("The build version cannot be empty.");
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("UpdateBuild");
            arguments.Append("-Version");
            arguments.AppendQuoted(version);

            StartAppVeyor(arguments);
        }

        /// <inheritdoc/>
        public void AddMessage(string message, AppVeyorMessageCategoryType category = AppVeyorMessageCategoryType.Information, string details = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new CakeException("The message cannot be empty.");
            }

            if (!IsRunningOnAppVeyor)
            {
                throw new CakeException("The current build is not running on AppVeyor.");
            }

            // Build the arguments.
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("AddMessage");
            arguments.AppendQuoted(message);
            arguments.Append("-Category");
            arguments.AppendQuoted(category.ToString());

            if (!string.IsNullOrWhiteSpace(details))
            {
                arguments.Append("-Details");
                arguments.AppendQuoted(details);
            }

            StartAppVeyor(arguments);
        }

        private void StartAppVeyor(ProcessArgumentBuilder arguments, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            var process = _processRunner.Start("appveyor", new ProcessSettings { Arguments = arguments });
            process.WaitForExit();
            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                throw new CakeException($"{memberName} failed ({exitCode}).");
            }
        }
    }
}