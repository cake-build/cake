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

        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on AppVeyor.; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information("Running on AppVeyor");
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information("Running on AppVeyor");
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public bool IsRunningOnAppVeyor => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("APPVEYOR"));

        /// <summary>
        /// Gets the AppVeyor environment.
        /// </summary>
        /// <value>
        /// The AppVeyor environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         ApiUrl: {0}
        ///         Configuration: {1}
        ///         JobId: {2}
        ///         JobName: {3}
        ///         Platform: {4}
        ///         ScheduledBuild: {5}",
        ///         BuildSystem.AppVeyor.Environment.ApiUrl,
        ///         BuildSystem.AppVeyor.Environment.Configuration,
        ///         BuildSystem.AppVeyor.Environment.JobId,
        ///         BuildSystem.AppVeyor.Environment.JobName,
        ///         BuildSystem.AppVeyor.Environment.Platform,
        ///         BuildSystem.AppVeyor.Environment.ScheduledBuild
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         ApiUrl: {0}
        ///         Configuration: {1}
        ///         JobId: {2}
        ///         JobName: {3}
        ///         Platform: {4}
        ///         ScheduledBuild: {5}",
        ///         AppVeyor.Environment.ApiUrl,
        ///         AppVeyor.Environment.Configuration,
        ///         AppVeyor.Environment.JobId,
        ///         AppVeyor.Environment.JobName,
        ///         AppVeyor.Environment.Platform,
        ///         AppVeyor.Environment.ScheduledBuild
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
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

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        public void UploadArtifact(FilePath path)
        {
            UploadArtifact(path, settings => settings.SetArtifactType(AppVeyorUploadArtifactType.Auto));
        }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settings">The settings to apply when uploading an artifact.</param>
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

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settingsAction">The settings to apply when uploading an artifact.</param>
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

        /// <summary>
        /// Uploads test results XML file to AppVeyor. Results type can be one of the following: mstest, xunit, nunit, nunit3, junit.
        /// </summary>
        /// <param name="path">The file path of the test results XML to upload.</param>
        /// <param name="resultsType">The results type. Can be mstest, xunit, nunit, nunit3 or junit.</param>
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

        /// <summary>
        /// Updates the build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     BuildSystem.AppVeyor.UpdateBuildVersion("2.0.0.0");
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     AppVeyor.UpdateBuildVersion("2.0.0.0");
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
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

        /// <summary>
        /// Adds a message to the AppVeyor build.  Messages can be categorised as: Information, Warning or Error.
        /// </summary>
        /// <param name="message">A short message to display.</param>
        /// <param name="category">The category of the message.</param>
        /// <param name="details">Additional message details.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     BuildSystem.AppVeyor.AddMessage(
        ///             "This is a error message.",
        ///             AppVeyorMessageCategoryType.Error,
        ///             "Error details."
        ///         );
        ///
        ///     BuildSystem.AppVeyor.AddMessage(
        ///             "This is a information message.",
        ///             AppVeyorMessageCategoryType.Information,
        ///             "Information details."
        ///         );
        ///
        ///     BuildSystem.AppVeyor.AddMessage(
        ///             "This is a warning message.",
        ///             AppVeyorMessageCategoryType.Warning,
        ///             "Warning details."
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     AppVeyor.AddMessage(
        ///             "This is a error message.",
        ///             AppVeyorMessageCategoryType.Error,
        ///             "Error details."
        ///         );
        ///
        ///     AppVeyor.AddMessage(
        ///             "This is a information message.",
        ///             AppVeyorMessageCategoryType.Information,
        ///             "Information details."
        ///         );
        ///
        ///     AppVeyor.AddMessage(
        ///             "This is a warning message.",
        ///             AppVeyorMessageCategoryType.Warning,
        ///             "Warning details."
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
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