// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AppVeyor.Data;
using Cake.Core.IO;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Represents a service that communicates with AppVeyor.
    /// </summary>
    public interface IAppVeyorProvider
    {
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
        bool IsRunningOnAppVeyor { get; }

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
        AppVeyorEnvironmentInfo Environment { get; }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        void UploadArtifact(FilePath path);

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settings">The settings to apply when uploading an artifact.</param>
        void UploadArtifact(FilePath path, AppVeyorUploadArtifactsSettings settings);

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        /// <param name="settingsAction">The settings to apply when uploading an artifact.</param>
        void UploadArtifact(FilePath path, Action<AppVeyorUploadArtifactsSettings> settingsAction);

        /// <summary>
        /// Uploads test results XML file to AppVeyor. Results type can be one of the following: mstest, xunit, nunit, nunit3, junit.
        /// </summary>
        /// <param name="path">The file path of the test results XML to upload.</param>
        /// <param name="resultsType">The results type. Can be mstest, xunit, nunit, nunit3 or junit.</param>
        void UploadTestResults(FilePath path, AppVeyorTestResultsType resultsType);

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
        void UpdateBuildVersion(string version);

        /// <summary>
        /// Adds a message to the AppVeyor build log.  Messages can be categorised as: Information, Warning or Error.
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
        void AddMessage(string message, AppVeyorMessageCategoryType category = AppVeyorMessageCategoryType.Information, string details = null);
    }
}