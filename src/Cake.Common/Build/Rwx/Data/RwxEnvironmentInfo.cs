// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX environment information for the current build.
    /// </summary>
    public sealed class RwxEnvironmentInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Run = new RwxRunInfo(environment);
            Task = new RwxTaskInfo(environment);
            Actor = new RwxActorInfo(environment);
            Git = new RwxGitInfo(environment);
            Runtime = new RwxRuntimeInfo(environment);
        }

        /// <summary>
        /// Gets RWX run information for the current build.
        /// </summary>
        /// <value>
        /// The run.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Run:
        ///         Id: {0}
        ///         Title: {1}
        ///         Url: {2}",
        ///         BuildSystem.Rwx.Environment.Run.Id,
        ///         BuildSystem.Rwx.Environment.Run.Title,
        ///         BuildSystem.Rwx.Environment.Run.Url
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Run:
        ///         Id: {0}
        ///         Title: {1}
        ///         Url: {2}",
        ///         Rwx.Environment.Run.Id,
        ///         Rwx.Environment.Run.Title,
        ///         Rwx.Environment.Run.Url
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        public RwxRunInfo Run { get; }

        /// <summary>
        /// Gets RWX task information for the current build.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Task:
        ///         Id: {0}
        ///         Url: {1}
        ///         AttemptNumber: {2}",
        ///         BuildSystem.Rwx.Environment.Task.Id,
        ///         BuildSystem.Rwx.Environment.Task.Url,
        ///         BuildSystem.Rwx.Environment.Task.AttemptNumber
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Task:
        ///         Id: {0}
        ///         Url: {1}
        ///         AttemptNumber: {2}",
        ///         Rwx.Environment.Task.Id,
        ///         Rwx.Environment.Task.Url,
        ///         Rwx.Environment.Task.AttemptNumber
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        public RwxTaskInfo Task { get; }

        /// <summary>
        /// Gets RWX actor information for the current build.
        /// </summary>
        /// <value>
        /// The actor.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Actor:
        ///         Id: {0}
        ///         Name: {1}",
        ///         BuildSystem.Rwx.Environment.Actor.Id,
        ///         BuildSystem.Rwx.Environment.Actor.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Actor:
        ///         Id: {0}
        ///         Name: {1}",
        ///         Rwx.Environment.Actor.Id,
        ///         Rwx.Environment.Actor.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        public RwxActorInfo Actor { get; }

        /// <summary>
        /// Gets RWX git information for the current build.
        /// </summary>
        /// <value>
        /// The git information. Populated by the RWX <c>git/clone</c> package, so it is only
        /// available to tasks that depend (directly or transitively) on a task that ran
        /// <c>git/clone</c>; otherwise the properties return empty strings.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Git:
        ///         RepositoryUrl: {0}
        ///         CommitSha: {1}
        ///         RefName: {2}",
        ///         BuildSystem.Rwx.Environment.Git.RepositoryUrl,
        ///         BuildSystem.Rwx.Environment.Git.CommitSha,
        ///         BuildSystem.Rwx.Environment.Git.RefName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Git:
        ///         RepositoryUrl: {0}
        ///         CommitSha: {1}
        ///         RefName: {2}",
        ///         Rwx.Environment.Git.RepositoryUrl,
        ///         Rwx.Environment.Git.CommitSha,
        ///         Rwx.Environment.Git.RefName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        public RwxGitInfo Git { get; }

        /// <summary>
        /// Gets RWX runtime information for the current build. Exposes the
        /// filesystem locations RWX uses for runtime-written output values and
        /// artifacts. Available inside any task running on RWX.
        /// </summary>
        /// <value>
        /// The runtime information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Runtime:
        ///         ValuesPath: {0}
        ///         ArtifactsPath: {1}",
        ///         BuildSystem.Rwx.Environment.Runtime.ValuesPath,
        ///         BuildSystem.Rwx.Environment.Runtime.ArtifactsPath
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information(
        ///         @"Runtime:
        ///         ValuesPath: {0}
        ///         ArtifactsPath: {1}",
        ///         Rwx.Environment.Runtime.ValuesPath,
        ///         Rwx.Environment.Runtime.ArtifactsPath
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        public RwxRuntimeInfo Runtime { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is continuous integration.
        /// </summary>
        /// <value>
        /// <c>true</c> if ci; otherwise, <c>false</c>.
        /// </value>
        public bool CI => GetEnvironmentBoolean("CI");

        /// <summary>
        /// Gets a value indicating whether the environment is RWX.
        /// </summary>
        /// <value>
        ///   <c>true</c> if RWX; otherwise, <c>false</c>.
        /// </value>
        public bool Rwx => GetEnvironmentBoolean("RWX");
    }
}
