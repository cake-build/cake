// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// Provides Go.CD environment information for a current build.
    /// </summary>
    public sealed class GoCDEnvironmentInfo : GoCDInfo
    {
        /// <summary>
        /// Gets GoCD pipeline information.
        /// </summary>
        /// <value>
        /// The GoCD pipeline information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Pipeline:
        ///         Name: {0}
        ///         Counter: {1}
        ///         Label: {2}",
        ///         BuildSystem.GoCD.Environment.Pipeline.Name,
        ///         BuildSystem.GoCD.Environment.Pipeline.Counter,
        ///         BuildSystem.GoCD.Environment.Pipeline.Label
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Pipeline:
        ///         Name: {0}
        ///         Counter: {1}
        ///         Label: {2}",
        ///         GoCD.Environment.Pipeline.Name,
        ///         GoCD.Environment.Pipeline.Counter,
        ///         GoCD.Environment.Pipeline.Label
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public GoCDPipelineInfo Pipeline { get; }

        /// <summary>
        /// Gets GoCD stage information.
        /// </summary>
        /// <value>
        ///   The GoCD stage information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Stage:
        ///         Name: {0}
        ///         Counter: {1}",
        ///         BuildSystem.GoCD.Environment.Stage.Name,
        ///         BuildSystem.GoCD.Environment.Stage.Counter
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Stage:
        ///         Name: {0}
        ///         Counter: {1}",
        ///         GoCD.Environment.Stage.Name,
        ///         GoCD.Environment.Stage.Counter
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public GoCDStageInfo Stage { get; }

        /// <summary>
        /// Gets GoCD repository information.
        /// </summary>
        /// <value>
        ///   The GoCD repository information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Revision: {0}
        ///         ToRevision: {1}
        ///         FromRevision: {2}",
        ///         BuildSystem.GoCD.Environment.Repository.Revision,
        ///         BuildSystem.GoCD.Environment.Repository.ToRevision,
        ///         BuildSystem.GoCD.Environment.Repository.FromRevision
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Revision: {0}
        ///         ToRevision: {1}
        ///         FromRevision: {2}",
        ///         GoCD.Environment.Repository.Revision,
        ///         GoCD.Environment.Repository.ToRevision,
        ///         GoCD.Environment.Repository.FromRevision
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public GoCDRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets the Go.CD URL.
        /// </summary>
        /// <value>
        /// The Go.CD URL.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"GoCDUrl: {0}",
        ///         BuildSystem.GoCD.Environment.GoCDUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"GoCDUrl: {0}",
        ///         GoCD.Environment.GoCDUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public string GoCDUrl => GetEnvironmentString("GO_SERVER_URL");

        /// <summary>
        /// Gets the environment name. This is only set if the environment is specified. Otherwise the variable is not set.
        /// </summary>
        /// <value>
        /// The environment name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"EnvironmentName: {0}",
        ///         BuildSystem.GoCD.Environment.EnvironmentName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"EnvironmentName: {0}",
        ///         GoCD.Environment.EnvironmentName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public string EnvironmentName => GetEnvironmentString("GO_ENVIRONMENT_NAME");

        /// <summary>
        /// Gets the name of the current job being run.
        /// </summary>
        /// <value>
        /// The job name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"JobName: {0}",
        ///         BuildSystem.GoCD.Environment.JobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"JobName: {0}",
        ///         GoCD.Environment.JobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public string JobName => GetEnvironmentString("GO_JOB_NAME");

        /// <summary>
        /// Gets the username of the user that triggered the build. This will have one of three possible values:
        /// anonymous - if there is no security.
        /// username of the user, who triggered the build.
        /// changes, if SCM changes auto-triggered the build.
        /// timer, if the pipeline is triggered at a scheduled time.
        /// </summary>
        /// <value>
        /// The username of the user that triggered the build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"User: {0}",
        ///         BuildSystem.GoCD.Environment.User
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GoCD.</para>
        /// <example>
        /// <code>
        /// if (GoCD.IsRunningOnGoCD)
        /// {
        ///     Information(
        ///         @"User: {0}",
        ///         GoCD.Environment.User
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GoCD");
        /// }
        /// </code>
        /// </example>
        public string User => GetEnvironmentString("GO_TRIGGER_USER");

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Pipeline = new GoCDPipelineInfo(environment);
            Stage = new GoCDStageInfo(environment);
            Repository = new GoCDRepositoryInfo(environment);
        }
    }
}