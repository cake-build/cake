// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines Build info for the current build.
    /// </summary>
    public sealed class AzurePipelinesBuildInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
            TriggeredBy = new AzurePipelinesTriggeredBy(environment);
        }

        /// <summary>
        /// Gets the a special variable that carries the security token used by the running build.
        /// </summary>
        /// <value>
        /// The security token.
        /// </value>
        public string AccessToken => GetEnvironmentString("SYSTEM_ACCESSTOKEN");

        /// <summary>
        /// Gets a value indicating whether more detailed logs to debug pipeline problems is enabled.
        /// </summary>
        /// <value>
        /// True if more detailed logs are enabled.
        /// </value>
        public bool Debug => GetEnvironmentBoolean("SYSTEM_DEBUG");

        /// <summary>
        /// Gets the local path on the agent where any artifacts are copied to before being pushed to their destination.
        /// </summary>
        /// <value>
        /// The path of the staging directory.
        /// </value>
        public DirectoryPath ArtifactStagingDirectory => GetEnvironmentString("BUILD_ARTIFACTSTAGINGDIRECTORY");

        /// <summary>
        /// Gets the local path on the agent you can use as an output folder for compiled binaries.
        /// </summary>
        /// <value>
        /// The path to the binaries directory.
        /// </value>
        public DirectoryPath BinariesDirectory => GetEnvironmentString("BUILD_BINARIESDIRECTORY");

        /// <summary>
        /// Gets the ID of the record for the completed build.
        /// </summary>
        /// <value>
        /// The ID of the record for the completed build.
        /// </value>
        public int Id => GetEnvironmentInteger("BUILD_BUILDID");

        /// <summary>
        /// Gets the name of the completed build.
        /// </summary>
        /// <remarks>You can specify the build number format that generates this value in the build definition.</remarks>
        /// <value>
        /// The name of the completed build.
        /// </value>
        public string Number => GetEnvironmentString("BUILD_BUILDNUMBER");

        /// <summary>
        /// Gets the URI for the build.
        /// </summary>
        /// <example><c>vstfs:///Build/Build/1430</c>.</example>
        /// <value>
        /// The URI for the build.
        /// </value>
        public Uri Uri => new Uri(GetEnvironmentString("BUILD_BUILDURI"));

        /// <summary>
        /// Gets the user who queued the build.
        /// </summary>
        /// <value>
        /// The user who queued the build.
        /// </value>
        public string QueuedBy => GetEnvironmentString("BUILD_QUEUEDBY");

        /// <summary>
        /// Gets the event that caused the build to run.
        /// </summary>
        /// <value>
        /// The event name.
        /// </value>
        public string Reason => GetEnvironmentString("BUILD_REASON");

        /// <summary>
        /// Gets the user the build was requested for.
        /// </summary>
        /// <value>
        /// The user the build was requested for.
        /// </value>
        public string RequestedFor => GetEnvironmentString("BUILD_REQUESTEDFOR");

        /// <summary>
        /// Gets the email of the user the build was requested for.
        /// </summary>
        /// <value>
        /// The email of the user the build was requested for.
        /// </value>
        public string RequestedForEmail => GetEnvironmentString("BUILD_REQUESTEDFOREMAIL");

        /// <summary>
        /// Gets the local path on the agent where your source code files are downloaded.
        /// </summary>
        /// <value>
        /// The source code directory.
        /// </value>
        public DirectoryPath SourcesDirectory => GetEnvironmentString("BUILD_SOURCESDIRECTORY");

        /// <summary>
        /// Gets the local path on the agent where any artifacts are copied to before being pushed to their destination.
        /// </summary>
        /// <value>
        /// The staging directory.
        /// </value>
        public DirectoryPath StagingDirectory => GetEnvironmentString("BUILD_STAGINGDIRECTORY");

        /// <summary>
        /// Gets local path on the agent where the test results are created.
        /// </summary>
        /// <value>
        /// The test result directory.
        /// </value>
        public DirectoryPath TestResultsDirectory => GetEnvironmentString("COMMON_TESTRESULTSDIRECTORY");

        /// <summary>
        /// Gets Azure Pipelines Build TriggeredBy information.
        /// </summary>
        /// <remarks>
        /// This is only populated if the build was triggered by another build.
        /// </remarks>
        /// <value>
        /// The Azure Pipelines Build Trigger information.
        /// </value>
        public AzurePipelinesTriggeredBy TriggeredBy { get; }
    }
}
